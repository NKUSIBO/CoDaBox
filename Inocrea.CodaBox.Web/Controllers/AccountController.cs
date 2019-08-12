
using System.Security.Claims;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.Web.Factory;
using Inocrea.CodaBox.Web.Helper;
using Inocrea.CodaBox.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RegisterModel = Inocrea.CodaBox.ApiModel.Models.RegisterModel;

namespace Inocrea.CodaBox.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IOptions<SettingsModels> _appSettings;

        public AccountController(IOptions<SettingsModels> app)
        {
            _appSettings = app;
            AppSettings.ApiUrl = _appSettings.Value.WebApiBaseUrl;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var saved = await ApiClientFactory.Instance.SaveUser(registerModel);
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var saved = await ApiClientFactory.Instance.LogUser(loginModel);
            if (saved.IsSuccess)
            {
                var userIdentity = new ClaimsIdentity("Custom");
                RedirectToAction("Index", "Home");
            }
            else
            {
                RedirectToAction("Login", "Account");
            }

            return Redirect("/");

        }
        public async Task<IActionResult> Logout()
        {
            var saved = await ApiClientFactory.Instance.LogOut();
            return Ok(saved.IsSuccess);
        }
    }
}