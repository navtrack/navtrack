using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services.Authentication
{
    [Service(typeof(IAuthenticationService))]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;
        private readonly IPasswordHasher passwordHasher;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService,
            IPasswordHasher passwordHasher)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
            this.passwordHasher = passwordHasher;
        }

        public async Task<ValidationResult> Login(LoginModel loginModel)
        {
            ValidationResult validationResult = new ValidationResult();

            User user = await userService.GetUserByEmail(loginModel.Email);

            if (user != null)
            {
                if (passwordHasher.CheckPassword(loginModel.Password, user.Salt, user.Hash))
                {
                    await SignIn(loginModel);
                }
                else
                {
                    validationResult.AddError(nameof(LoginModel.Password), "Invalid password provided.");
                }
            }
            else
            {
                validationResult.AddError(nameof(LoginModel.Email), "Invalid email provided.");
            }

            return validationResult;
        }

        private async Task SignIn(LoginModel loginModel)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginModel.Email)
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