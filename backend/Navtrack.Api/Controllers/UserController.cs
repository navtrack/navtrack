using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.User;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
public class UserController(IUserService userService) : UserControllerBase(userService)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpGet(ApiPaths.User)]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<UserModel> Get()
    {
        UserModel currentUser = await userService.GetCurrentUser();

        return currentUser;
    }
}