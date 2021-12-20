using System.Threading.Tasks;
using Navtrack.Api.Model.Users;

namespace Navtrack.Api.Services.Users;

public interface IUserService
{
    Task<CurrentUserModel> GetCurrentUser();
    Task UpdateUser(UpdateUserModel model);
}