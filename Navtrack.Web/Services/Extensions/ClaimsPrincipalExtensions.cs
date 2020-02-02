using System.Linq;
using System.Security.Claims;
using static System.Int32;

namespace Navtrack.Web.Services.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal claimsPrincipal)
        {
            Claim nameIdentifier = claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);

            int userId = Parse(nameIdentifier.Value);

            return userId;
        }
    }
}