using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Navtrack.Api.Services.IdentityServer.Model;
using Navtrack.Api.Services.Mappers.Users;
using Navtrack.Common.Settings;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer.ExternalAuthentication;

[Service(typeof(IExtensionGrantValidator))]
public class AppleExtensionGrantValidator : IExtensionGrantValidator
{
    private readonly IUserDataService userDataService;
    private readonly IExternalLoginHandler externalLoginHandler;
    private readonly ISettingService settingService;

    public AppleExtensionGrantValidator(IUserDataService userDataService, IExternalLoginHandler externalLoginHandler,
        ISettingService settingService)
    {
        this.userDataService = userDataService;
        this.externalLoginHandler = externalLoginHandler;
        this.settingService = settingService;
    }

    public string GrantType => "apple";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        AppleAuthenticationSettings settings = await settingService.Get<AppleAuthenticationSettings>();

        string? userId = await externalLoginHandler.HandleToken(new HandleTokenInput(settings)
        {
            Token = context.Request.Raw["code"],
            IdClaimType = ClaimTypes.NameIdentifier,
            EmailClaimType = ClaimTypes.Email,
            GetUser = userDataService.GetByEmailOrAppleId,
            Map = (email, id) => UserDocumentMapper.MapWithExternalId(email, appleId: id),
            ExternalId = user => user.AppleId,
            SetId = (userDocumentId, id) => userDataService.Update(userDocumentId, new UpdateUser
            {
                AppleId = id
            })
        });

        context.Result = !string.IsNullOrEmpty(userId)
            ? new GrantValidationResult(userId, "apple", new List<Claim>())
            : new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid token.");
    }
}