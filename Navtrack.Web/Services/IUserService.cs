using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services
{
    public interface IUserService : IGenericService<User, UserModel>
    {
        Task<User> GetUserByEmail(string email);
        Task<UserModel> GetAuthenticatedUser(string? email);
        Task<bool> EmailIsUsed(string email);
    }
}