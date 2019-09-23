
using System;
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
        readonly UserManager<CodaBoxUser> _userManager;
        readonly SignInManager<CodaBoxUser> _signInManager;
        readonly IConfiguration _configuration;
        readonly ILogger<AccountController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CodaBoxContext _context;



        public AccountController(
            UserManager<CodaBoxUser> userManager,
            SignInManager<CodaBoxUser> signInManager,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountController> logger)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
        }

        private readonly IOptions<SettingsModels> _appSettings;


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(registerModel.Username);

                if (user == null)
                {
                    var myuser = new CodaBoxUser()
                    {
                        //TODO: Use Automapper instaed of manual binding  

                        UserName = registerModel.Username,
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName,
                        Email = registerModel.Email,
                        CompanyId = registerModel.CompanyId,
                    };


                    var identityResult = await this._userManager.CreateAsync(user, registerModel.Password);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View();


        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var codaUser = await _userManager.FindByNameAsync(model.Username);

                var result = await _signInManager.PasswordSignInAsync(codaUser, "P@ssW0rd", false, false);
                if (result.Succeeded)
                {
                    RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Result = "error :" + result;
                }
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IActionResult> Logout()
        {
            var saved = await ApiClientFactory.Instance.LogOut();
            return Ok(saved.IsSuccess);
        }
    }
}