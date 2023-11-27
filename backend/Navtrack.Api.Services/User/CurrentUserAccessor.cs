using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.IdentityServer;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(ICurrentUserAccessor))]
public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IUserRepository userRepository;
    private UserDocument? currentUser;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userRepository = userRepository;
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

    public async Task<UserDocument> Get()
    {
        if (currentUser == null)
        {
            ObjectId id = GetId();

            currentUser = await userRepository.GetById(id);
        }

        return currentUser;
    }
}