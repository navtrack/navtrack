using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.User;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
public abstract class UserControllerBase(IUserService userService, IUserAccessService accessService)
    : ControllerBase
{
    protected readonly IUserService userService = userService;

    [HttpPost(ApiPaths.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> RegisterAccount([FromBody] RegisterAccountModel model)
    {
        await userService.Register(model);

        return Ok();
    }


    [HttpPatch(ApiPaths.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<IActionResult> Update([FromBody] UpdateUserModel model)
    {
        await userService.Update(model);

        return Ok();
    }

    [HttpPost(ApiPaths.UserPasswordChange)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        await accessService.ChangePassword(model);

        return Ok();
    }

    [HttpPost(ApiPaths.UserPasswordReset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        await accessService.ResetPassword(model);

        return Ok();
    }

    [HttpPost(ApiPaths.UserPasswordForgot)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] ForgotPasswordModel model)
    {
        await accessService.ForgotPassword(model);

        return Ok();
    }
}