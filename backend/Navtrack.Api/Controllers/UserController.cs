using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.User;

namespace Navtrack.Api.Controllers;

public class UserController(IRequestHandler requestHandler) : UserControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.User)]
    [ProducesResponseType(typeof(CurrentUser), StatusCodes.Status200OK)]
    public async Task<CurrentUser> Get()
    {
        CurrentUser result =
            await requestHandler.Handle<GetCurrentUserRequest, CurrentUser>(new GetCurrentUserRequest());

        return result;
    }
}