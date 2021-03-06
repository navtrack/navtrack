using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Navtrack.Api.Model.Models;
using Navtrack.Api.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer
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
            if (user.Role != null)
            {
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, ((UserRole) user.Role).ToString()));
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}