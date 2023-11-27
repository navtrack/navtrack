using System.Threading.Tasks;
using Navtrack.Api.Model.User;

namespace Navtrack.Api.Services.User;

public interface IUserService
{
    Task<Model.User.UserModel> GetCurrentUser();
    Task Register(RegisterAccountModel model);
    Task Update(UpdateUserModel model);
}