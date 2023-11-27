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
public abstract class UserControllerBase : ControllerBase
{
    protected readonly IUserService userService;
    private readonly IUserAccessService userAccessService;

    protected UserControllerBase(IUserService userService, IUserAccessService userAccessService)
    {
        this.userService = userService;
        this.userAccessService = userAccessService;
    }

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
        await userAccessService.ChangePassword(model);

        return Ok();
    }

    [HttpPost(ApiPaths.UserPasswordReset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        await userAccessService.ResetPassword(model);

        return Ok();
    }

    [HttpPost(ApiPaths.UserPasswordForgot)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] ForgotPasswordModel model)
    {
        await userAccessService.ForgotPassword(model);

        return Ok();
    }
}