using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
using Navtrack.Web.Services;
using Navtrack.Web.Services.Extensions;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;
        private readonly IUserService userService;

        public AccountController(IAccountService accountService, IUserService userService)
        {
            this.accountService = accountService;
            this.userService = userService;
        }

        [HttpGet]
        public Task<UserModel> Get()
        {
            return userService.Get(User.GetId());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            ValidationResult validationResult = await accountService.Register(registerModel);

            return ValidationResult(validationResult);
        }
    }
}