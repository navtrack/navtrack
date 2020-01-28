using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
using Navtrack.Web.Services;
using Navtrack.Web.Services.Authentication;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUserService userService;

        public AccountController(IAuthenticationService authenticationService, IUserService userService)
        {
            this.authenticationService = authenticationService;
            this.userService = userService;
        }

        [HttpGet]
        public Task<UserModel> Get()
        {
            return userService.GetAuthenticatedUser(User.Identity.Name);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            ValidationResult validationResult = await authenticationService.Login(loginModel);

            return ValidationResult(validationResult);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            ValidationResult validationResult = await authenticationService.Register(registerModel);

            return ValidationResult(validationResult);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await authenticationService.Logout();

            return Ok();
        }
    }
}