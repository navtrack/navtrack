using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;

namespace Navtrack.Web.Mappers
{
    [Service(typeof(IMapper<User, UserModel>))]
    [Service(typeof(IMapper<UserModel, User>))]
    public class UserMapper : IMapper<User, UserModel>, IMapper<UserModel, User>
    {
        public UserModel Map(User source, UserModel destination)
        {
            destination.Id = source.Id;
            destination.Email = source.Email;

            return destination;
        }

        public User Map(UserModel source, User destination)
        {
            destination.Id = source.Id;
            destination.Email = source.Email;

            return destination;
        }
    }
}