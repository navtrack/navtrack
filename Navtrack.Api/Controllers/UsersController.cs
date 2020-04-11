using Microsoft.AspNetCore.Authorization;
using Navtrack.Api.Models;
using Navtrack.Api.Services;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : GenericController<User, UserModel>
    {
        public UsersController(IUserService userService) : base(userService)
        {
        }
    }
}