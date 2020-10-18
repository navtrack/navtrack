using System;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Accounts;

namespace Navtrack.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public class AccountController : BaseController
    {
        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public Task<IActionResult> Register([FromBody] RegisterAccountRequestModel model)
        {
            return HandleCommand(model);
        }
    }
}