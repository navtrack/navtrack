using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Model.Account;
using Navtrack.Web.Services;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
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
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            await accountService.Login(loginModel);
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await accountService.Logout();

            return Ok();
        }
    }
}