using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services
{
    [Service(typeof(IUserService))]
    [Service(typeof(IGenericService<Device, DeviceModel>))]
    public class UserService : GenericService<User, UserModel>, IUserService
    {
        public UserService(IRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await repository.GetEntities<User>().FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public async Task<UserModel> GetAuthenticatedUser(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            User user = await repository.GetEntities<User>().FirstOrDefaultAsync(x => x.Email == email);

            return user != null ? mapper.Map<User, UserModel>(user) : null;
        }

        public Task<bool> EmailIsUsed(string email)
        {
            return repository.GetEntities<User>().AnyAsync(x => x.Email == email);
        }

    }
}