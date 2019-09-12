
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Entities;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.Web.Factory;
using Inocrea.CodaBox.Web.Helper;
using Inocrea.CodaBox.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegisterModel = Inocrea.CodaBox.ApiModel.Models.RegisterModel;

namespace Inocrea.CodaBox.Web.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;
        readonly IConfiguration configuration;
        readonly ILogger<AccountController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;



        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        private readonly IOptions<SettingsModels> _appSettings;

        
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