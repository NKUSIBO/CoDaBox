using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebCodaBox.Models;

namespace WebCodaBox.Controllers
{
    public class HomeController : Controller
    {
       
            private readonly IOptions<SettingsModels> _appSettings;

            private static List<StatementAccountViewModel> _listData = new List<StatementAccountViewModel>();
            private static string _name = "";

            public HomeController(IOptions<SettingsModels> app)
            {
                _appSettings = app;
            }

            public ActionResult Index()
            {

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


