using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inocrea.CodaBox.Web.Models;
using Inocrea.CodaBox.Web.Helper;
using Inocrea.CodaBox.ApiModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Inocrea.CodaBox.Web.Factory;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IOptions<SettingsModels> _appSettings;
      
        private static List<StatementAccountViewModel> _listData = new List<StatementAccountViewModel>();
        private static string _name = "";

        public HomeController(IOptions<SettingsModels> app)
        {
            _appSettings = app;
            AppSettings.ApiUrl = _appSettings.Value.WebApiBaseUrl;
        }
       
        public  ActionResult Index()
        {
            
            return View();
        }
        public static DateTime StringToDate(string Date)
        {
            try
            {
                return DateTime.Parse(Date);
            }
            catch (FormatException)
            {
                return DateTime.Parse("1/1/0001");
            }
        }
        private List<StatementAccountViewModel> ProcessCollection(List<StatementAccountViewModel> lstElements, Microsoft.AspNetCore.Http.IFormCollection requestFormData)
        {
            string searchText = string.Empty;
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };
            if (requestFormData.TryGetValue("search[value]", out tempOrder))
            {
                searchText = requestFormData["search[value]"].ToString();
            }
            tempOrder = new[] { "" };
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetProperty(columName);
                        if (sortDirection == "asc")
                        {
                            return lstElements
                                .Where(x => x.Date.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower())||x.Iban.ToString().ToLower().Contains(searchText.ToLower())
                                            || string.Equals(x.InitialBalance.ToString(CultureInfo.CurrentCulture).ToLower(), searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) || string.Equals(x.NewBalance.ToString(CultureInfo.CurrentCulture).ToLower(), searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase))
                                .Skip(skip)
                                .Take(pageSize)
                                .OrderBy(prop.GetValue).ToList();
                        }
                        // x.InitialBalance.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower())
                        return lstElements
                            .Where(x => x.Date.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower()) || x.Iban.ToString().ToLower().Contains(searchText.ToLower())
                                        || string.Equals(x.InitialBalance.ToString(CultureInfo.CurrentCulture).ToLower(), searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) || string.Equals(x.NewBalance.ToString(CultureInfo.CurrentCulture).ToLower(), searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase))
                            .Skip(skip)
                            .Take(pageSize)
                            .OrderByDescending(prop.GetValue).ToList();
                    }

                    return lstElements;
                }
            }
            return null;
        }
        private PropertyInfo GetProperty(string columnName)
        {
            var properties = typeof(StatementAccountViewModel).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLower().Equals(columnName.ToLower()))
                {
                    prop = item;
                    break;

                }
            }

            return prop;
        }
        private int GetTotalRecordsFiltered(IFormCollection requestFormData, List<StatementAccountViewModel> lstItems, List<StatementAccountViewModel> listProcessedItems)
        {
            var recFiltered = 0;
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };
            if (requestFormData.TryGetValue("search[value]", out tempOrder))
            {
                if (string.IsNullOrEmpty(requestFormData["search[value]"].ToString().Trim()))
                {
                    recFiltered = lstItems.Count;
                }
                else
                {
                    recFiltered = listProcessedItems.Count;
                }
            }
            return recFiltered;

        }
        public FileResult ExportTransactions()
        {
            // query data from database  
            DataTable dt = ExportToExcel.ExportGenericTransactions<List<StatementAccountViewModel>>(_listData);
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                if ((dt.Columns[i].ColumnName.ToString().Contains("DATE") ||
                     (dt.Columns[i].ColumnName.ToString().Contains("Date"))))
                {
                     _name = dt.Columns[i].ColumnName;
                    if (!(dt.Columns[i].ColumnName.ToString().Contains("FORMAT")))
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName.ToString());
                    }

                }


            }

            var fileName = "trans"+"transactions" + ".xlsx"; //declaration.xlsx";


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public void InsertStatementToDb(List<Statements> staInsertResult)
        {
            ApiServerCoda _api = new ApiServerCoda();
            HttpClient client = _api.Initial();
            foreach (var item in staInsertResult)
            {
                var statementToInsert = JsonConvert.SerializeObject(item);
                var content = new StringContent(statementToInsert, System.Text.Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage result = client.PostAsync("api/Statements", content).Result;
                    if (result.IsSuccessStatusCode)
                    {

                        Console.WriteLine("All is fine");

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }
        [HttpPost]
        public async Task<IActionResult> LoadTransaction()
        {
           
            var requestFormData = Request.Form;
            List<StatementAccountViewModel> data = await ApiClientFactory.Instance.GetStatements();
            try
            {
                _listData = ProcessCollection(data, requestFormData);
                int transFiltered = GetTotalRecordsFiltered(requestFormData, data, _listData);
                dynamic response = new
                {
                    data = _listData,
                    draw = requestFormData["draw"],
                    recordsFiltered = transFiltered,
                    recordsTotal = data.Count
                };
                return Ok(response);

             
            }
            catch (Exception)
            {

                return NotFound();
            }



        }
      
        [HttpPost]
       
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
