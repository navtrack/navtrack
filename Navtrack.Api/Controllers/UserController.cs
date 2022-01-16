using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Users;
using Navtrack.Api.Services.Users;

namespace Navtrack.Api.Controllers;

[ApiController]
[Route("user")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(CurrentUserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<CurrentUserModel> Get()
    {
        CurrentUserModel currentUser = await userService.GetCurrentUser();

        return currentUser;
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateUserModel model)
    {
        await userService.UpdateUser(model);

        return Ok();
    }
}