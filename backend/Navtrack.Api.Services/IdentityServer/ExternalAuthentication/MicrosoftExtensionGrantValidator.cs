using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Navtrack.Api.Services.IdentityServer.Model;
using Navtrack.Api.Services.Mappers;
using Navtrack.Common.Settings;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer.ExternalAuthentication;

[Service(typeof(IExtensionGrantValidator))]
public class MicrosoftExtensionGrantValidator : IExtensionGrantValidator
{
    private readonly IUserDataService userDataService;
    private readonly IExternalLoginHandler externalLoginHandler;
    private readonly ISettingService settingService;

    public MicrosoftExtensionGrantValidator(IUserDataService userDataService,
        IExternalLoginHandler externalLoginHandler, ISettingService settingService)
    {
        this.userDataService = userDataService;
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
            GetUser = userDataService.GetByMicrosoftId,
            Map = (email, id) => UserDocumentMapper.MapWithExternalId(email, id),
            ExternalId = user => user.MicrosoftId,
            SetId = userDataService.SetMicrosoftId
        });

        context.Result = !string.IsNullOrEmpty(userId)
            ? new GrantValidationResult(userId, "microsoft", new List<Claim>())
            : new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid token.");
    }
}