using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer;

[Service(typeof(IExternalLoginHandler))]
public class ExternalLoginHandler : IExternalLoginHandler
{
    private readonly IUserDataService userDataService;

    public ExternalLoginHandler(IUserDataService userDataService)
    {
        this.userDataService = userDataService;
    }

    public async Task<string?> HandleToken(HandleTokenInput input)
    {
        ClaimsPrincipal? claimPrincipal = await GetClaimsPrincipal(input);

        if (claimPrincipal != null)
        {
            Claim? id = claimPrincipal.FindFirst(input.IdClaimType);
            Claim? email = claimPrincipal.FindFirst(input.EmailClaimType);
            Claim? isPrivateEmail = claimPrincipal.FindFirst("is_private_email");
            bool isPrivateEmailValue = isPrivateEmail?.Value == "true";

            if (!string.IsNullOrEmpty(id?.Value) && !string.IsNullOrEmpty(email?.Value))
            {
                UserDocument? userDocument = await input.GetUser(email.Value, id.Value);

                if (userDocument == null)
                {
                    userDocument = input.Map(email.Value, id.Value);

                    await userDataService.Add(userDocument);
                }
                else
                {
                    if (string.IsNullOrEmpty(input.ExternalId(userDocument)))
                    {
                        await input.SetId(userDocument.Id, id.Value);
                    }
                    if (!isPrivateEmailValue && userDocument.Email != email.Value)
                    {
                        await userDataService.UpdateUser(userDocument, email.Value);
                    }
                }

                return userDocument.Id.ToString();
            }
        }

        return null;
    }

    private static async Task<ClaimsPrincipal?> GetClaimsPrincipal(HandleTokenInput input)
    {
        try
        {
            ConfigurationManager<OpenIdConnectConfiguration> openidConfigManaged = new(input.MetadataAddress,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever());

            OpenIdConnectConfiguration config = await openidConfigManaged.GetConfigurationAsync();

            TokenValidationParameters parameters = new()
            {
                RequireAudience = true,
                RequireExpirationTime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidAudiences = new[] { input.ValidAudience },
                ValidIssuers = new[] { input.ValidIssuer },
                IssuerSigningKeys = config.SigningKeys
            };

            JwtSecurityTokenHandler tokenHandler = new();

            ClaimsPrincipal? claimPrincipal = tokenHandler.ValidateToken(input.Token, parameters, out _);

            return claimPrincipal;
        }
        catch (Exception)
        {
            // authentication failed, ignore
        }

        return null;
    }
}