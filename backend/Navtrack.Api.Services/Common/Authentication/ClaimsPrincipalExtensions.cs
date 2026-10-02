using System.Linq;
using System.Security.Claims;
using OpenIddict.Abstractions;

namespace Navtrack.Api.Services.Common.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal claimsPrincipal)
    {
        Claim? subject = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == OpenIddictConstants.Claims.Subject);

        return subject?.Value;
    }
}