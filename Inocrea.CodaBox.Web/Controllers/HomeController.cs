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
using System.IO;
using System.Reflection;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Inocrea.CodaBox.Web.Factory;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Inocrea.CodaBox.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IOptions<SettingsModels> _appSettings;
        private static List<InvoiceModel> listInvoice = new List<InvoiceModel>();
        private static List<Transactions> dataList = new List<Transactions>();
        private static string name = "";

        public HomeController(IOptions<SettingsModels> app)
        {
            _appSettings = app;
            AppSettings.ApiUrl = "Https://" + _appSettings.Value.WebApiBaseUrl;
        }
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
        public FileResult ExportTransactions()
        {
            // query data from database  
            DataTable dt = ExportToExcel.ExportGenericTransactions<List<InvoiceModel>>(dataList);
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                if ((dt.Columns[i].ColumnName.ToString().Contains("DATE") ||
                     (dt.Columns[i].ColumnName.ToString().Contains("Date"))))
                {
                    var name = dt.Columns[i].ColumnName;
                    if (!(dt.Columns[i].ColumnName.ToString().Contains("FORMAT")))
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName.ToString());
                    }

                }


            }

            var fileName = name + ".xlsx"; //declaration.xlsx";


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

        public FileResult Export()
        {
            DataTable dt = ExportToExcel.ExportGenericInvoiceModel<List<InvoiceModel>>(listInvoice);
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                if ((dt.Columns[i].ColumnName.ToString().Contains("DATE") ||
                     (dt.Columns[i].ColumnName.ToString().Contains("Date"))))
                {
                    var name = dt.Columns[i].ColumnName;
                    if (!(dt.Columns[i].ColumnName.ToString().Contains("FORMAT")))
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName.ToString());
                    }

                }


            }

            var fileName = name + ".xlsx"; //declaration.xlsx";


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
        [HttpPost]
        //public async Task<IActionResult> LoadTransaction()
        //{
        //    var requestFormData = Request.Form;

        //    List<Transactions> data = await ApiClientFactory.Instance.GetInvoice();
        //    dataList = data;
        //    try
        //    {
        //        var listData =  ProcessModuleCollection(data, requestFormData);
        //        dynamic response = new
        //        {
        //            Data = listData,
        //            Draw = requestFormData["draw"],
        //            RecordsFiltered = data.Count,
        //            RecordsTotal = data.Count
        //        };
                
        //        return  Ok(response);

        //        //return Json(response);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest();
        //    }


            
        //}

        //private object ProcessModuleCollection(List<Transactions> listData, IFormCollection requestFormData)
        //{
        //    var skip = Convert.ToInt32(requestFormData["start"].ToString());
        //    var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
        //  StringValues tempOrder = new[] {""};
        //    if(requestFormData.TryGetValue("order[0][column]",out tempOrder))
        //    {
        //        var columnIndex = requestFormData["order[0][column]"].ToString();
        //        var sortDirection = requestFormData["order[0][dir]"].ToString();
        //        tempOrder = new[] {""};
        //        if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
        //        {
        //            var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
        //            if (pageSize > 0)
        //            {
        //                var prop = getProperty(columnName);
        //                if (sortDirection == "asc")
        //                {
        //                    return listData.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                }

        //                return listData.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();


        //            }

        //            return listData;




        //        }
        //    }

        //    return null;
        //}

        //private PropertyInfo getProperty(string columnName)
        //{
        //    var properties = typeof(Transactions).GetProperties();
        //    PropertyInfo prop = null;
        //    foreach (var item in properties)
        //    {
        //        if (item.Name.ToLower().Equals(columnName.ToLower()))
        //        {
        //            prop = item;
        //            break;

        //        }
        //    }

        //    return prop;
        //}
        //public IActionResult LoadData()
        //{
        //    try
        //    {
        //        var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

        //        // Skip number of Rows count  
        //        var start = Request.Form["start"].FirstOrDefault();

        //        // Paging Length 10,20  
        //        var length = Request.Form["length"].FirstOrDefault();

        //        // Sort Column Name  
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

        //        // Sort Column Direction (asc, desc)  
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

        //        // Search Value from (Search box)  
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault();

        //        //Paging Size (10, 20, 50,100)  
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;

        //        int skip = start != null ? Convert.ToInt32(start) : 0;

        //        int recordsTotal = 0;

        //        // getting all Customer data  
               
        //        //Sorting  
        //        //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        //        //{
        //        //    dataList = dataList.OrderBy<(sortColumn + " " + sortColumnDirection);
        //        //}
        //        //Search  
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            dataList = (List<Transactions>) dataList.Where(m => m.Name == searchValue);
        //        }

        //        //total number of rows counts   
        //        recordsTotal = dataList.Count();
        //        //Paging   
        //        var data = dataList.Skip(skip).Take(pageSize).ToList();
        //        //Returning Json Data  
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
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
