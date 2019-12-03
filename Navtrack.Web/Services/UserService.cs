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
    public class UserService : GenericService<User, UserModel>,  IUserService
    {
        private readonly IRepository repository;

        public UserService(IRepository repository, IMapper mapper) : base(repository, mapper)
        {
            this.repository = repository;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await repository.GetEntities<User>().FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }
    }
}