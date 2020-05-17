using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer
{
    [Service(typeof(IResourceOwnerPasswordValidator))]
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserService userService;
        private readonly IPasswordHasher passwordHasher;

        public ResourceOwnerPasswordValidator(IUserService userService, IPasswordHasher passwordHasher)
        {
            this.userService = userService;
            this.passwordHasher = passwordHasher;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            UserEntity user = await userService.GetUserByEmail(context.UserName);

            if (user != null && passwordHasher.CheckPassword(context.Password, user.Hash, user.Salt))
            {
                context.Result = new GrantValidationResult($"{user.Id}",
                    "custom",
                    new List<Claim>());
            }
            else
            {
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }
    }
}