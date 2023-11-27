using System.Threading.Tasks;
using Navtrack.Api.Model.User;

namespace Navtrack.Api.Services.User;

public interface IUserAccessService
{
    Task ForgotPassword(ForgotPasswordModel model);
    Task ChangePassword(ChangePasswordModel model);
    Task ResetPassword(ResetPasswordModel model);
}