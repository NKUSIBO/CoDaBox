using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebCodaBox.Factory;
using WebCodaBox.Helper;
using WebCodaBox.Models;

namespace WebCodaBox.Controllers
{
    public class TransactionsController : Controller
    {
        public static int _index;
        private readonly IOptions<SettingsModels> _appSettings;
        DateTime dateIn=new DateTime();
        DateTime dateOut = new DateTime();
        private  static List<TransactionsAccountViewModel> _listData = new List<TransactionsAccountViewModel>();

        private static string _name = "";

        public TransactionsController(IOptions<SettingsModels> app)
        {
            _appSettings = app;
            AppSettings.ApiUrl = _appSettings.Value.WebApiBaseUrl;
        }
        // GET: Transactions
        public  ActionResult Index(int index)
        {
            _index = index;
            return View();
        }

        //public async Task<ActionResult> IndexByDate(DateTime? datepickerStart, DateTime? datepickerEnd)
        //{
        //    List<TransactionsAccountViewModel> data = await ApiClientFactory.Instance.GetTransactionsByDateAsync(_index, datepickerStart, datepickerEnd);
        //    try
        //    {
        //        _listData.Clear();
        //        _listData = ProcessCollection(data, requestFormData);
        //        int transFiltered = GetTotalRecordsFiltered(requestFormData, data, _listData);
        //        dynamic response = new
        //        {
        //            data = _listData,
        //            draw = requestFormData["draw"],
        //            recordsFiltered = transFiltered,
        //            recordsTotal = data.Count
        //        };
        //        ViewBag.Tra = data;
        //        return View(data);


        //    }
        //    catch (Exception ex)
        //    {

        //        return NotFound();
        //    }


        //    return View();
        //}
        public async Task<ActionResult> IndexByDate(string Iban,DateTime? datepickerStart, DateTime? datepickerEnd)
        {
            List<TransactionsAccountViewModel> data = await ApiClientFactory.Instance.GetTransactionsByDateIban(Iban, datepickerStart, datepickerEnd);
            try
            {
                _listData.Clear();
                _listData = ProcessCollection(data, requestFormData);
                int transFiltered = GetTotalRecordsFiltered(requestFormData, data, _listData);
                dynamic response = new
                {
                    data = _listData,
                    draw = requestFormData["draw"],
                    recordsFiltered = transFiltered,
                    recordsTotal = data.Count
                };
                ViewBag.Tra = data;
                return View(data);


            }
            catch (Exception ex)
            {

                return NotFound();
            }


            return View();
        }
        ApiServerCoda _api = new ApiServerCoda();

        private static List<TransactionsAccountViewModel> listData = new List<TransactionsAccountViewModel>();
        private static string name = "";
        private static IFormCollection requestFormData;

        public async Task<IActionResult> TransactionsByStatements(int statementId)

        {
             requestFormData = Request.Form;
            List<TransactionsAccountViewModel> data = await ApiClientFactory.Instance.GetTransactions(_index);
            try
            {
                _listData.Clear();
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
            catch (Exception )
            {

                return NotFound();
            }



           

        }
        [HttpPost]
        public async Task<ActionResult> TransactionByDate(string iban,DateTime? datepickerStart, DateTime? datepickerEnd)
        {


            
            List<TransactionsAccountViewModel> data =await  ApiClientFactory.Instance.GetTransactionsByDateIban(iban, datepickerStart, datepickerEnd);
            try
            {
                _listData.Clear();
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
            catch (Exception ex)
            {

                return NotFound();
            }


           
        }
        [HttpPost]
        public async Task<ActionResult> TransactionByDateIban(string iban,DateTime? datepickerStart, DateTime? datepickerEnd)
        {



            List<TransactionsAccountViewModel> data = await ApiClientFactory.Instance.GetTransactionsByDateIban(iban, datepickerStart, datepickerEnd);
            try
            {
                _listData.Clear();
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
            catch (Exception ex)
            {

                return NotFound();
            }



        }
        private List<TransactionsAccountViewModel> ProcessCollection(List<TransactionsAccountViewModel> lstElements, Microsoft.AspNetCore.Http.IFormCollection requestFormData)
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
                                .Where(x => x.ValueDate==StringToDate(searchText)
                                            || x.Amount.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchText.ToLower()) || x.Message.ToString().ToLower().Contains(searchText.ToLower())||x.Iban.ToLower().Contains(searchText.ToLower()))
                                .Skip(skip)
                                .Take(pageSize)
                                .OrderBy(prop.GetValue).ToList();
                        }

                        return lstElements
                                .Where(x => x.ValueDate == StringToDate(searchText)
                                            || x.Amount.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchText.ToLower()) || x.Message.ToString().ToLower().Contains(searchText.ToLower()) || x.Iban.ToLower().Contains(searchText.ToLower()))
                                .Take(pageSize)
                                .OrderByDescending(prop.GetValue).ToList();
                    }

                    return lstElements;
                }
            }
            return null;
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
        private PropertyInfo GetProperty(string columnName)
        {
            var properties = typeof(TransactionsAccountViewModel).GetProperties();
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
        private int GetTotalRecordsFiltered(IFormCollection requestFormData, List<TransactionsAccountViewModel> lstItems, List<TransactionsAccountViewModel> listProcessedItems)
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
            DataTable dt = ExportToExcel.ExportTransactions<List<TransactionsAccountViewModel>>(_listData);
            for (int i = 0; i < dt.Columns.Count; i++)
            {

               
                    name = dt.Columns[i].ColumnName;
                    switch (name)
                    {
                        case "TransactionDate": dt.Columns.Remove(dt.Columns[i].ColumnName);
                            break;

                        case "ValueDate":
                        dt.Columns.Remove(dt.Columns[i].ColumnName);
                        break;
                        case "Date":
                            dt.Columns[i].ColumnName = "Date";
                            dt.Columns.Remove("ValueDate");
                        break;
                        case "Amount":
                            dt.Columns[i].ColumnName = "Montant";
                            break;
                        case "Bic":
                            dt.Columns[i].ColumnName = "Bic";
                            break;
                       
                        case "StructuredMessage":
                            dt.Columns[i].ColumnName = "MessageStructuree";
                            break;
                        case "CurrencyCode":
                            dt.Columns[i].ColumnName = "CurrencyCode";
                            break;
                        case "Iban":
                            dt.Columns[i].ColumnName = "CompteBancaire";
                            break;
                        default:
                          dt.Columns.Remove(dt.Columns[i].ColumnName);
                          break;

                }


            }

            var fileName = "trans" + "transactions" + ".xlsx"; //declaration.xlsx";


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

    }
}
