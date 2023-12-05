using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Navtrack.Api.Services.IdentityServer.Model;
using Navtrack.Api.Services.Mappers.Users;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Services.Settings;

namespace Navtrack.Api.Services.IdentityServer.ExternalAuthentication;

[Service(typeof(IExtensionGrantValidator))]
public class MicrosoftExtensionGrantValidator(
    IUserRepository repository,
    IExternalLoginHandler loginHandler,
    ISettingService service)
    : IExtensionGrantValidator
{
    public string GrantType => "microsoft";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        MicrosoftAuthenticationSettings settings = await service.Get<MicrosoftAuthenticationSettings>();

        string userId = await loginHandler.HandleToken(new HandleTokenInput(settings)
        {
            Token = context.Request.Raw["code"],
            IdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier",
            EmailClaimType = ClaimTypes.Email,
            GetUser = repository.GetByEmailOrMicrosoftId,
            Map = (email, id) => UserDocumentMapper.MapWithExternalId(email, id),
            ExternalId = user => user.MicrosoftId,
            SetId = (userId, id) => repository.Update(userId, new UpdateUser
            {
                MicrosoftId = id
            })
        });

        context.Result = !string.IsNullOrEmpty(userId)
            ? new GrantValidationResult(userId, "microsoft", new List<Claim>())
            : new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid token.");
    }
}