using System.Threading.Tasks;
using Navtrack.Api.Model.Models;

namespace Navtrack.Api.Services.Users
{
    public interface IUserService
    {
        Task<UserModel> Get(int userId);
    }
}