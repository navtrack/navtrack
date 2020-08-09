using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Services
{
    public interface IUserDataService : IGenericDataService<UserEntity>
    {
        Task<UserEntity> GetUserByEmail(string email);
        Task<bool> EmailIsUsed(string email);
    }
}