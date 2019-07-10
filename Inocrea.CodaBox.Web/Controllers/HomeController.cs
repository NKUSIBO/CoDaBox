using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel;
using Inocrea.CodaBox.Web.Factory;
using Microsoft.AspNetCore.Mvc;
using Inocrea.CodaBox.Web.Models;
using Microsoft.Extensions.Options;
using Inocrea.CodaBox.Web.Helper;
using OfficeOpenXml;

namespace Inocrea.CodaBox.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<SettingsModels> _appSettings;

        public HomeController(IOptions<SettingsModels> app)
        {
            _appSettings = app;
            AppSettings.ApiUrl = "Https://"+_appSettings.Value.WebApiBaseUrl;
        }
        public async Task<IActionResult> Index()
        {

            List<InvoiceModel> data = await ApiClientFactory.Instance.GetInvoice();
            return View(data);
        }
        public async Task<IActionResult> ExportV2(CancellationToken cancellationToken)
        {
            // query data from database  
            await Task.Yield();
            var list = await ApiClientFactory.Instance.GetInvoice();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(list, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

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
