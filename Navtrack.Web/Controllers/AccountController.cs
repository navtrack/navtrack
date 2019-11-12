using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Model.Account;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public UserInfo UserInfo()
        {
            return new UserInfo
            {
                Email = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated
            };
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Email)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
            {
                IsPersistent = loginModel.RememberMe
            });
            
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await httpContextAccessor.HttpContext.SignOutAsync();
            
            return Ok();
        }
    }
}