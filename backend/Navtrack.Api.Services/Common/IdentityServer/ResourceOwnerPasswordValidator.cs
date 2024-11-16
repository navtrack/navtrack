using System.Threading.Tasks;
using IdentityServer4.Validation;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.IdentityServer;

[Service(typeof(IResourceOwnerPasswordValidator))]
public class ResourceOwnerPasswordValidator(IPasswordHasher hasher, IUserRepository repository) : IResourceOwnerPasswordValidator
{
    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        UserDocument? user = await repository.GetByEmail(context.UserName);

        if (user is { Password: not null } && hasher.CheckPassword(context.Password, user.Password.Hash, user.Password.Salt))
        {
            context.Result = new GrantValidationResult(user.Id.ToString(), "custom", []);
        }
        else
        {
            context.Result = GrantValidationResultMapper.Map(ApiErrorCodes.User_000004_InvalidUsernameOrPassword);
        }
    }
}