using System.Threading.Tasks;
using Navtrack.Api.Model.User;

namespace Navtrack.Api.Services.User;

public interface IUserAccessService
{
    Task ForgotPassword(ForgotPasswordRequest model);
    Task ChangePassword(ChangePasswordRequest model);
    Task ResetPassword(ResetPasswordRequest model);
}