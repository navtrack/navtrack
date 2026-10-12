using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;
using OpenIddict.Abstractions;

namespace Navtrack.Api.Services.Common.Authentication;

[Service(typeof(IAuthenticationGrantHandler))]
public class PasswordGrantHandler(IPasswordHasher hasher, IUserRepository repository) : IAuthenticationGrantHandler
{
    public string GrantType => OpenIddictConstants.GrantTypes.Password;

    public async Task<AuthenticationGrantResult> Authenticate(OpenIddictRequest request)
    {
        if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
        {
            UserEntity? user = await repository.GetByEmail(request.Username);

            if (user != null && !string.IsNullOrEmpty(user.PasswordHash) &&
                !string.IsNullOrEmpty(user.PasswordSalt) &&
                hasher.CheckPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthenticationGrantResult(UserId: user.Id.ToString());
            }
        }

        return new AuthenticationGrantResult(Error: ApiErrorCodes.User_InvalidUsernameOrPassword);
    }
}