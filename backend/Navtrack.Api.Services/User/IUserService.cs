using System.Threading.Tasks;
using Navtrack.Api.Model.User;

namespace Navtrack.Api.Services.User;

public interface IUserService
{
    Task<UserModel> GetCurrentUser();
    Task Register(RegisterAccountRequest model);
    Task Update(UpdateUserRequest model);
}