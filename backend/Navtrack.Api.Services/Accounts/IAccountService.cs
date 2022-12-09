using System.Threading.Tasks;
using Navtrack.Api.Model.Accounts;

namespace Navtrack.Api.Services.Accounts;

public interface IAccountService
{
    Task ChangePassword(ChangePasswordModel model);
    Task Register(RegisterAccountModel model);
}