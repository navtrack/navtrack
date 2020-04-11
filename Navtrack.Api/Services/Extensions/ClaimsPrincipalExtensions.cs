using System.Linq;
using System.Security.Claims;
using IdentityModel;
using static System.Int32;

namespace Navtrack.Api.Services.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal claimsPrincipal)
        {
            Claim nameIdentifier = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);

            return nameIdentifier != null && TryParse(nameIdentifier.Value, out int userId) ? userId : 0;
        }
    }
}