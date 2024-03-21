using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Account;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Account;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
public abstract class AccountControllerBase(IAccountService accountService)
    : ControllerBase
{
    [HttpPost(ApiPaths.Account)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] RegisterAccountModel model)
    {
        await accountService.Register(model);

        return Ok();
    }

    [HttpPost(ApiPaths.AccountPasswordForgot)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {
        await accountService.ForgotPassword(model);

        return Ok();
    }
    
    [HttpPost(ApiPaths.AccountPasswordReset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        await accountService.ResetPassword(model);

        return Ok();
    }
    
    [HttpPost(ApiPaths.AccountPasswordChange)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        await accountService.ChangePassword(model);

        return Ok();
    }
}