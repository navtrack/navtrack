﻿using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.User;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
public abstract class UserControllerBase(IUserService userService) : ControllerBase
{
    [HttpPatch(ApiPaths.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<IActionResult> Update([FromBody] UpdateUserModel model)
    {
        await userService.Update(model);

        return Ok();
    }
}