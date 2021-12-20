using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Accounts;
using Navtrack.Api.Services.Accounts;

namespace Navtrack.Api.Controllers;

[Route("account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    [HttpPost("password")]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> ChangePassword([FromBody]ChangePasswordModel model)
    {
        await accountService.ChangePassword(model);

        return Ok();
    }
        
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> RegisterAccount([FromBody]RegisterAccountModel model)
    {
        await accountService.Register(model);

        return Ok();
    }
}