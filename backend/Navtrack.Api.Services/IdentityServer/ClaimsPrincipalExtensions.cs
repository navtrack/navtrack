using System.Linq;
using System.Security.Claims;
using IdentityModel;

namespace Navtrack.Api.Services.IdentityServer;

public static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal claimsPrincipal)
    {
        Claim subject = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);

        return subject?.Value;
    }
}