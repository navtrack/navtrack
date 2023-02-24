using MongoDB.Bson;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Roles;

[Service(typeof(IRoleService))]
public class RoleService : IRoleService
{
    private readonly ICurrentUserAccessor currentUserAccessor;

    public RoleService(ICurrentUserAccessor currentUserAccessor)
    {
        this.currentUserAccessor = currentUserAccessor;
    }

    public void CheckRole(AssetDocument asset, AssetRoleType assetRoleType)
    {
        ObjectId currentUserId = currentUserAccessor.GetId();
            
        // if (asset.UserRoles.All(x => x.UserId != currentUserId))
        // {
        //     throw new ApiException(HttpStatusCode.Forbidden);
        // }
    }
}