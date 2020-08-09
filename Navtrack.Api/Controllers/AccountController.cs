using System;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Accounts;
using Navtrack.Api.Model.Accounts.Requests;
using Navtrack.Api.Model.Models;
using Navtrack.Api.Services.Extensions;

namespace Navtrack.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public class AccountController : BaseController
    {
        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public Task<ActionResult<AccountInfoResponseModel>> Get()
        {
            return HandleRequest<AccountInfoRequest, AccountInfoResponseModel>(new AccountInfoRequest
            {
                UserId = User.GetId()
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public Task<IActionResult> Register([FromBody] RegisterAccountModel model)
        {
            return HandleRequest(model);
        }
    }
}