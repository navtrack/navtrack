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
public class CurrentUserAccessor(IHttpContextAccessor contextAccessor, IUserRepository repository)
    : ICurrentUserAccessor
{
    private UserDocument? currentUser;

    public ObjectId? GetId()
    {
        string? id = contextAccessor.HttpContext?.User.GetId();

        ObjectId? currentUserId = string.IsNullOrEmpty(id) ? null : ObjectId.Parse(id);

        return currentUserId;
    }

    public async Task<UserDocument> Get()
    {
        if (currentUser == null)
        {
            ObjectId? id = GetId();

            if (id == null)
            {
                throw new ApiException(HttpStatusCode.Unauthorized);
            }

            currentUser = await repository.GetById(id.Value);
        }

        return currentUser;
    }
}