using System.Threading.Tasks;
using Navtrack.Api.Model.Account;

namespace Navtrack.Api.Services.Account;

public interface IAccountService
{
    Task Register(RegisterAccountModel model);
    Task ForgotPassword(ForgotPasswordModel model);
    Task ChangePassword(ChangePasswordModel model);
    Task ResetPassword(ResetPasswordModel model);
}