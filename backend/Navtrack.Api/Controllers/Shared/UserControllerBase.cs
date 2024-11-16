using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Account;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.User;

namespace Navtrack.Api.Controllers.Shared;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class UserControllerBase(IRequestHandler requestHandler)
    : ControllerBase
{
    [HttpPost(ApiPaths.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateUserModel model)
    {
        await requestHandler.Handle(new UpdateUserRequest
        {
            Model = model
        });
    
        return Ok();
    }

    [HttpPost(ApiPaths.UserChangePassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        await requestHandler.Handle(new ChangePasswordRequest
        {
            Model = model
        });

        return Ok();
    }
}