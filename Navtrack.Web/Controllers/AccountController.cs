using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Authentication;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpGet("userInfo")]
        public UserInfo UserInfo()
        {
            return new UserInfo
            {
                Email = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            ValidationResult validationResult = await authenticationService.Login(loginModel);

            if (validationResult.IsValid)
            {
                return Ok();
            }

            return BadRequest(new ErrorModel(validationResult));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await authenticationService.Logout();

            return Ok();
        }
    }
}