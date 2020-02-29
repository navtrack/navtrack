using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Navtrack.Library.DI;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Extensions;

namespace Navtrack.Web.Services.IdentityServer
{
    [Service(typeof(IProfileService))]
    public class ProfileService : IProfileService
    {
        private readonly IUserService userService;

        public ProfileService(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            int userId = context.Subject.GetId();
            UserModel user = await userService.Get(userId);
            
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}