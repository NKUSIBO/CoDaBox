using System;

using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using Identity.Entities;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.ApiServer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Microsoft.AspNetCore.Http;

namespace Inocrea.CodaBox.ApiServer.Controllers
{
     [Produces("application/json")]
        [Route("api/[controller]")]
        [ApiController]
        [AllowAnonymous]
        public class AccountController : ControllerBase
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

            private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        [HttpPost]
            [Route("token")]
            public async Task<IActionResult> CreateToken([FromBody] LoginModel model)
            {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var claim = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                   
                };
                var signinKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Site"],
                    audience: configuration["Jwt:Site"],
                    expires: DateTime.Now.AddHours(expiryInMinutes),
                    claims: claim,
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
                
                return Ok(
                    new LoginModel
                    {
                        Username = model.Username,
                        Password = model.Password,

                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    });
            }
            return Unauthorized();

        }

            [Authorize]
            [HttpPost]
            [Route("refreshtoken")]
            public async Task<IActionResult> RefreshToken()
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = await userManager.FindByNameAsync(
                    User.Identity.Name ??
                    User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
                    );
                return Ok(GetToken(user));

            }


            [HttpPost]
            [Route("register")]
            [AllowAnonymous]
            public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        //TODO: Use Automapper instaed of manual binding  

                        UserName = registerModel.Username,
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName,
                        Email = registerModel.Email,
                        CompanyId=registerModel.CompanyId,
                    };

                try
                {
                    var identityResult = await this.userManager.CreateAsync(user, registerModel.Password);
                    if (identityResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(identityResult.Errors);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                }
                return BadRequest(ModelState);


            }

            private String GetToken(IdentityUser user)
          {
            var utcNow = DateTime.Now.AddHours(1);

            using (RSA privateRsa = RSA.Create())
            {
                privateRsa.FromXmlFile(Path.Combine(Directory.GetCurrentDirectory(),
                    "Keys",
                    this.configuration.GetValue<string>("Tokens:PrivateKey")
                ));
                var privateKey = new RsaSecurityKey(privateRsa);
                SigningCredentials signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);


                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
                };

                var jwt = new JwtSecurityToken(
                    signingCredentials: signingCredentials,
                    claims: claims,
                    notBefore: utcNow.AddHours(1),
                    expires: DateTime.Now.AddHours(3),
                    audience: this.configuration.GetValue<string>("Tokens:Audience"),
                    issuer: this.configuration.GetValue<string>("Tokens:Issuer")
                );

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }

          }


           [HttpPost]
           [Route("Logout")]
           [AllowAnonymous]
            public async Task<IActionResult> Logout()
            {
                await signInManager.SignOutAsync();
                return Ok();

            }

    }
    
}