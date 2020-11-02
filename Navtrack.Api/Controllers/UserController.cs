using System;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.IdentityServer;

namespace Navtrack.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public class UserController : BaseController
    {
        public UserController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public Task<ActionResult<UserResponseModel>> Get()
        {
            return HandleCommand<GetCurrentUserCommand, UserResponseModel>(new GetCurrentUserCommand());
        }

        [HttpPatch]
        public Task<IActionResult> Update([FromBody] UpdateUserRequestModel model)
        {
            return HandleCommand(new UpdateUserCommand
            {
                UserId = User.GetId(),
                Model = model
            });
        }
    }
}