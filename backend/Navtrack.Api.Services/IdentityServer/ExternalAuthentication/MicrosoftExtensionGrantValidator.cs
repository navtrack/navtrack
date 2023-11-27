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
public class MicrosoftExtensionGrantValidator : IExtensionGrantValidator
{
    private readonly IUserRepository userRepository;
    private readonly IExternalLoginHandler externalLoginHandler;
    private readonly ISettingService settingService;

    public MicrosoftExtensionGrantValidator(IUserRepository userRepository,
        IExternalLoginHandler externalLoginHandler, ISettingService settingService)
    {
        this.userRepository = userRepository;
        this.externalLoginHandler = externalLoginHandler;
        this.settingService = settingService;
    }

    public string GrantType => "microsoft";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        MicrosoftAuthenticationSettings settings = await settingService.Get<MicrosoftAuthenticationSettings>();

        string userId = await externalLoginHandler.HandleToken(new HandleTokenInput(settings)
        {
            Token = context.Request.Raw["code"],
            IdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier",
            EmailClaimType = ClaimTypes.Email,
            GetUser = userRepository.GetByEmailOrMicrosoftId,
            Map = (email, id) => UserDocumentMapper.MapWithExternalId(email, id),
            ExternalId = user => user.MicrosoftId,
            SetId = (userId, id) => userRepository.Update(userId, new UpdateUser
            {
                MicrosoftId = id
            })
        });

        context.Result = !string.IsNullOrEmpty(userId)
            ? new GrantValidationResult(userId, "microsoft", new List<Claim>())
            : new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid token.");
    }
}