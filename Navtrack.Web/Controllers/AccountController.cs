using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models.Account;
using Navtrack.Web.Services;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
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
            await accountService.Login(loginModel);
            
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await accountService.Logout();

            return Ok();
        }
    }
}