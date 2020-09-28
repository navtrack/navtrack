using System;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Commands;
using Navtrack.Api.Model.User;

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
        public Task<ActionResult<UserModel>> Get()
        {
            return HandleCommand<GetCurrentUserCommand, UserModel>(new GetCurrentUserCommand());
        }
    }
}