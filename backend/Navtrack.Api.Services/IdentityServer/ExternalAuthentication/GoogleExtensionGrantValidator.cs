using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
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
public class GoogleExtensionGrantValidator : IExtensionGrantValidator
{
    private readonly IUserDataService userDataService;
    private readonly IExternalLoginHandler externalLoginHandler;
    private readonly ISettingService settingService;

    public GoogleExtensionGrantValidator(IUserDataService userDataService, IExternalLoginHandler externalLoginHandler,
        ISettingService settingService)
    {
        this.userDataService = userDataService;
        this.externalLoginHandler = externalLoginHandler;
        this.settingService = settingService;
    }

    public string GrantType => "google";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        GoogleAuthenticationSettings settings = await settingService.Get<GoogleAuthenticationSettings>();

        string idToken = await GetIdToken(context, settings);
        
        string? userId = await externalLoginHandler.HandleToken(new HandleTokenInput(settings)
        {
            Token = idToken,
            IdClaimType = ClaimTypes.NameIdentifier,
            EmailClaimType = ClaimTypes.Email,
            GetUser = userDataService.GetByEmailOrGoogleId,
            Map = (email, id) => UserDocumentMapper.MapWithExternalId(email, googleId: id),
            ExternalId = user => user.GoogleId,
            SetId = (userId, id) => userDataService.Update(userId, new UpdateUser
            {
                GoogleId = id
            })
        });

        context.Result = !string.IsNullOrEmpty(userId)
            ? new GrantValidationResult(userId, "google", new List<Claim>())
            : new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid token.");
    }

    private static async Task<string> GetIdToken(ExtensionGrantValidationContext context,
        GoogleAuthenticationSettings settings)
    {
        if (context.Request.Client.ClientId == IdentityServerConfig.NavtrackMobileClientId)
        {
            return context.Request.Raw["code"];
        }
        
        GoogleAuthorizationCodeFlow flow = new(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret
            }
        });
        
        TokenResponse tokenResponse =
            await flow.ExchangeCodeForTokenAsync("", context.Request.Raw["code"], "postmessage", CancellationToken.None);

        return tokenResponse.IdToken;
    }
}