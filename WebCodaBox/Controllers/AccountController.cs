using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebCodaBox.Models;

namespace WebCodaBox.Controllers
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
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            try
            {
                var codaUser = await _userManager.FindByNameAsync(model.Username);

                var result = await _signInManager.PasswordSignInAsync(codaUser, "P@ssW0rd", false, false);
                if (result.Succeeded)
                {
                    return Redirect(Url.Action("Index", "Home", new { returnUrl }));
                }
                else
                {
                    ViewBag.Result = "error :" + result;
                }
                return View( );
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}