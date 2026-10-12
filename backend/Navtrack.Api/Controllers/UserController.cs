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
    [ProducesResponseType(typeof(CurrentUserModel), StatusCodes.Status200OK)]
    public Task<CurrentUserModel> Get() =>
        Query<GetCurrentUserRequest, CurrentUserModel>(new GetCurrentUserRequest());
}
