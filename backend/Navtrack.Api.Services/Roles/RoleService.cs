using MongoDB.Bson;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Roles;

[Service(typeof(IRoleService))]
public class RoleService(ICurrentUserAccessor userAccessor) : IRoleService
{
    public void CheckRole(AssetDocument asset, AssetRoleType assetRoleType)
    {
        ObjectId currentUserId = userAccessor.GetId();
            
        // if (asset.UserRoles.All(x => x.UserId != currentUserId))
        // {
        //     throw new ApiException(HttpStatusCode.Forbidden);
        // }
    }
}