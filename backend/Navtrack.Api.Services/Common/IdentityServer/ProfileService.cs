using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.IdentityServer;

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