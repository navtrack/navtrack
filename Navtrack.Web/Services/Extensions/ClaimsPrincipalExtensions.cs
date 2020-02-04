using System.Linq;
using System.Security.Claims;
using static System.Int32;

namespace Navtrack.Web.Services.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal claimsPrincipal)
        {
            Claim nameIdentifier = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return nameIdentifier != null && TryParse(nameIdentifier.Value, out int userId) ? userId : 0;
        }
    }
}