using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer;

[Service(typeof(IProfileService))]
public class ProfileService : IProfileService
{
    private readonly IUserDataService userDataService;

    public ProfileService(IUserDataService userDataService)
    {
        this.userDataService = userDataService;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string? userId = context.Subject.GetId();
            
        UserDocument user = await userDataService.GetByObjectId(ObjectId.Parse(userId));

        context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}