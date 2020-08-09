using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Models;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Users
{
    [Service(typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public UserService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UserModel> Get(int userId)
        {
            UserEntity entity = await repository.GetEntities<UserEntity>().FirstOrDefaultAsync(x => x.Id == userId);

            return entity != null ? mapper.Map<UserEntity, UserModel>(entity) : null;
        }
    }
}