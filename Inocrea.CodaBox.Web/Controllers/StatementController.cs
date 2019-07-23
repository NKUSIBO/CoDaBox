using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Inocrea.CodaBox.Web.Factory;
using Inocrea.CodaBox.Web.Helper;
using Inocrea.CodaBox.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Inocrea.CodaBox.Web.Controllers
{
   
    public class StatementController : Controller
    {
        private readonly IOptions<SettingsModelsApiServer> _apiServerSettings;
        public StatementController(IOptions<SettingsModelsApiServer> app)
        {
            _apiServerSettings = app;
            ApiServerSettings.StatementsUrl = "Https://" + _apiServerSettings.Value.StatementsUrl;
        }
    
        public async Task<IActionResult> Index()
        {
            Statements sta=new Statements();
            Message<Statements> data = await ApiServerFactory.Instance.SaveStatement(sta);
            return View();
        }
    }
}