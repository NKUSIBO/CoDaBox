using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inocrea.CodaBox.ApiServer.Entities;
using Inocrea.CodaBox.Web.Helper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.Web.Controllers
{
    public class TransactionsController : Controller
    {
        public static int _index;
       
        // GET: Transactions
        public  ActionResult Index(int index)
        {
            _index = index;
            return View();
        }

        ApiServerCoda _api = new ApiServerCoda();

        private static List<TransactionsAccountViewModel> listData = new List<TransactionsAccountViewModel>();
        private static string name = "";
        public async Task<CompteBancaire> GetCompte(int id)
        {
            CompteBancaire role = new CompteBancaire();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync(("api/CompteBancaires/" + id));
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                role = JsonConvert.DeserializeObject<CompteBancaire>(result);
            }

            return role;
        }
        public async Task<IActionResult> TransactionsByStatements(int statementId)

        {
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync(("api/Transactions/GetTransactionsByStatement/"+_index));
            var dataVm = new List<Transactions>();
            var data = new List<TransactionsAccountViewModel>();
            var requestFormData = Request.Form;
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                try
                {

                    dataVm = JsonConvert.DeserializeObject<List<Transactions>>(result);
                    foreach (var transact in dataVm)
                    {
                        CompteBancaire cp = GetCompte(transact.CompteBancaireId).Result;
                        TransactionsAccountViewModel tav=new TransactionsAccountViewModel();
                        tav.Iban = cp.Iban;
                        tav.Bic = cp.Bic;
                        tav.CurrencyCode = cp.CurrencyCode;
                        tav.IdentificationNumber = cp.IdentificationNumber;
                        tav.Message = transact.Message;
                        tav.Amount = transact.Amount;
                        tav.StructuredMessage = transact.StructuredMessage;
                        tav.TransactionDate = transact.TransactionDate;
                        tav.ValueDate = transact.ValueDate;
                        data.Add(tav);
                    }

                    listData = ProcessCollection(data, requestFormData);
                    int transFiltered = GetTotalRecordsFiltered(requestFormData, data, listData);
                    dynamic response = new
                    {
                        data = listData,
                        draw = requestFormData["draw"],
                        recordsFiltered = transFiltered,
                        recordsTotal = data.Count
                    };
                    return Ok(response);
                }
                catch (Exception ex)
                {

                    return BadRequest();
                }
            }


            return NotFound();

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
                                .Where(x => x.ValueDate.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower())
                                            || x.TransactionDate.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower()) || x.StructuredMessage.ToString().ToLower().Contains(searchText.ToLower()))
                                .Skip(skip)
                                .Take(pageSize)
                                .OrderBy(prop.GetValue).ToList();
                        }

                        return lstElements
                            .Where(x => x.ValueDate.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower())
                                        || x.TransactionDate.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower()) || x.StructuredMessage.ToString().ToLower().Contains(searchText.ToLower()))
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
            DataTable dt = ExportToExcel.ExportTransactions<List<TransactionsAccountViewModel>>(listData);
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                if ((dt.Columns[i].ColumnName.ToString().Contains("DATE") ||
                     (dt.Columns[i].ColumnName.ToString().Contains("Date"))))
                {
                    name = dt.Columns[i].ColumnName;
                    if (!(dt.Columns[i].ColumnName.ToString().Contains("FORMAT")))
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName.ToString());
                    }

                }


            }

            var fileName = "trans" + "transactions" + ".xlsx"; //declaration.xlsx";


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    try
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
        }

    }
}
