
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Inocrea.CodaBox.Web.Models;
using Inocrea.CodaBox.Web.Helper;



using Microsoft.Extensions.Options;

using Inocrea.CodaBox.ApiModel.ViewModel;


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
