using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Navtrack.Library.DI;
using Navtrack.Web.Model.Account;

namespace Navtrack.Web.Services
{
    [Service(typeof(IAccountService))]
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task Login(LoginModel loginModel)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Email)
            };

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                {
                    IsPersistent = loginModel.RememberMe
                });
        }

        public Task Logout()
        {
            return httpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}