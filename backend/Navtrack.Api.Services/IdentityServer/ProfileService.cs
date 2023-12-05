using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.IdentityServer;

[Service(typeof(IProfileService))]
public class ProfileService(IUserRepository repository) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string? userId = context.Subject.GetId();
            
        UserDocument user = await repository.GetById(userId);

        context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}