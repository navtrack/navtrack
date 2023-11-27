using System.Net.Mime;
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

public class UserController : UserControllerBase
{
    public UserController(IUserService userService, IUserAccessService userAccessService) : base(userService,
        userAccessService)
    {
    }

    [HttpGet(ApiPaths.User)]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<UserModel> Get()
    {
        UserModel currentUser = await userService.GetCurrentUser();

        return currentUser;
    }
}