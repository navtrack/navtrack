using Navtrack.DataAccess.Model;
using Navtrack.Web.Models;
using Navtrack.Web.Services;

namespace Navtrack.Web.Controllers
{
    public class UsersController : GenericController<User, UserModel>
    {
        public UsersController(IUserService userService) : base(userService)
        {
        }
    }
}