using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.IdentityServer;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(ICurrentUserAccessor))]
public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IUserDataService userDataService;
    private UserDocument? currentUser;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IUserDataService userDataService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userDataService = userDataService;
    }

    public ObjectId GetId()
    {
        string? id = httpContextAccessor.HttpContext?.User.GetId();

        if (string.IsNullOrEmpty(id))
        {
            throw new ApiException(HttpStatusCode.Unauthorized);
        }
            
        ObjectId currentUserId = ObjectId.Parse(id);

        return currentUserId;
    }

    public async Task<UserDocument> GetCurrentUser()
    {
        if (currentUser == null)
        {
            ObjectId id = GetId();

            currentUser = await userDataService.GetByObjectId(id);
        }

        return currentUser;
    }
}