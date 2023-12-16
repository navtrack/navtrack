using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.User;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
public abstract class AccountControllerBase(IAccountService accountService, IUserAccessService accessService)
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
        await accessService.ForgotPassword(model);

        return Ok();
    }
    
    [HttpPost(ApiPaths.AccountPasswordReset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        await accessService.ResetPassword(model);

        return Ok();
    }
    
    [HttpPost(ApiPaths.AccountPasswordChange)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        await accessService.ChangePassword(model);

        return Ok();
    }
}