using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Inocrea.CodaBox.Web.Controllers.Api
{
    public class AccountControllerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}