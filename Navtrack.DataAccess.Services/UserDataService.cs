using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services
{
    [Service(typeof(IUserDataService))]
    public class UserDataService : GenericDataService<UserEntity>, IUserDataService
    {
        private readonly IRepository repository;

        public UserDataService(IRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<UserEntity> GetUserByEmail(string email)
        {
            email = email.ToLower();
            
            UserEntity user = await repository.GetEntities<UserEntity>().FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public Task<bool> EmailIsUsed(string email)
        {
            email = email.ToLower();
            
            return repository.GetEntities<UserEntity>().AnyAsync(x => x.Email == email);
        }

        public Task<UserEntity> GetUserById(int id)
        {
            return repository.GetEntities<UserEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}