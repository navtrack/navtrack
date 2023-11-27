using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Services.Passwords;

namespace Navtrack.Api.Services.IdentityServer;

[Service(typeof(IResourceOwnerPasswordValidator))]
public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly IPasswordHasher passwordHasher;
    private readonly IUserRepository userRepository;

    public ResourceOwnerPasswordValidator(IPasswordHasher passwordHasher, IUserRepository userRepository)
    {
        this.passwordHasher = passwordHasher;
        this.userRepository = userRepository;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        UserDocument? user = await userRepository.GetByEmail(context.UserName);

        if (user != null && passwordHasher.CheckPassword(context.Password, user.Password.Hash, user.Password.Salt))
        {
            context.Result = new GrantValidationResult($"{user.Id}",
                "custom",
                new List<Claim>());
        }
        else
        {
            context.Result =
                new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password.");
        }
    }
}