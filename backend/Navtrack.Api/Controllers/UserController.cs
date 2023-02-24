using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.User;

namespace Navtrack.Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IUserAccessService userAccessService;

    public UserController(IUserService userService, IUserAccessService userAccessService)
    {
        this.userService = userService;
        this.userAccessService = userAccessService;
    }

    [HttpGet(ApiPaths.User)]
    [ProducesResponseType(typeof(CurrentUserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<CurrentUserModel> Get()
    {
        CurrentUserModel currentUser = await userService.GetCurrentUser();

        return currentUser;
    }

    [HttpPost(ApiPaths.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> RegisterAccount([FromBody] RegisterAccountRequest model)
    {
        await userService.Register(model);

        return Ok();
    }


    [HttpPatch(ApiPaths.User)]
    [ProducesResponseType(typeof(CurrentUserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest model)
    {
        await userService.UpdateUser(model);

        return Ok();
    }

    [HttpPost(ApiPaths.UserPasswordChange)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
    {
        await userAccessService.ChangePassword(model);

        return Ok();
    }

    [HttpPost(ApiPaths.UserPasswordReset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
    {
        await userAccessService.ResetPassword(model);

        return Ok();
    }

    [HttpPost(ApiPaths.UserPasswordForgot)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] ForgotPasswordRequest model)
    {
        await userAccessService.ForgotPassword(model);

        return Ok();
    }
}