using System.Threading.Tasks;
using Navtrack.Api.Model.User;

namespace Navtrack.Api.Services.User;

public interface IUserService
{
    Task<CurrentUserModel> GetCurrentUser();
    Task Register(RegisterAccountRequest model);
    Task UpdateUser(UpdateUserRequest model);
}