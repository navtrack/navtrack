using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
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
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService,
            IPasswordHasher passwordHasher, IRepository repository, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
            this.passwordHasher = passwordHasher;
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ValidationResult> Login(LoginModel loginModel)
        {
            ValidationResult validationResult = new ValidationResult();

            User user = await userService.GetUserByEmail(loginModel.Email);

            if (user != null)
            {
                if (passwordHasher.CheckPassword(loginModel.Password, user.Hash, user.Salt))
                {
                    await SignIn(user);
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

        private async Task SignIn(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Email, user.Email)
            };

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authenticationProperties);
        }

        public Task Logout()
        {
            return httpContextAccessor.HttpContext.SignOutAsync();
        }

        public async Task<ValidationResult> Register(RegisterModel registerModel)
        {
            ValidationResult validationResult = new ValidationResult();

            if (await userService.EmailIsUsed(registerModel.Email))
            {
                validationResult.AddError(nameof(RegisterModel.Email), "Email is already used.");
            }

            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                validationResult.AddError(nameof(RegisterModel.ConfirmPassword), "The passwords must match.");
            }

            if (validationResult.IsValid)
            {
                using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

                User user = mapper.Map<RegisterModel, User>(registerModel);
                
                unitOfWork.Add(user);

                await unitOfWork.SaveChanges();
            }

            return validationResult;
        }
    }
}