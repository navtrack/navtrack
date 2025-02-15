using System.Linq;
using System.Security.Claims;
using Duende.IdentityModel;

namespace Navtrack.Api.Services.Common.IdentityServer;

public static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal claimsPrincipal)
    {
        Claim? subject = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);

        return subject?.Value;
    }
}