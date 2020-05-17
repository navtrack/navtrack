using System.Threading.Tasks;
using Navtrack.Api.Models;
using Navtrack.Api.Services.Generic;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Services
{
    public interface IUserService : IGenericService<UserEntity, UserModel>
    {
        Task<UserEntity> GetUserByEmail(string email);
        Task<bool> EmailIsUsed(string email);
    }
}