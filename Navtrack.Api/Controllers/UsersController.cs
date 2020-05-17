using Microsoft.AspNetCore.Authorization;
using Navtrack.Api.Models;
using Navtrack.Api.Services;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : GenericController<UserEntity, UserModel>
    {
        public UsersController(IUserService userService) : base(userService)
        {
        }
    }
}