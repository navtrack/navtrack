using System.Linq;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Assets;

public static class AssetAuthorize
{
    public static bool UserHasAssetRole(UserDocument user, string assetId, AssetRoleType assetRoleType)
    {
        AssetRoleType[] validRoles = assetRoleType == AssetRoleType.Viewer
            ? [AssetRoleType.Owner, AssetRoleType.Viewer]
            : [assetRoleType];

        ObjectId assetObjectId = ObjectId.Parse(assetId);

        bool isAuthorized =
            (user.AssetRoles?.Any(x => x.AssetId == assetObjectId && validRoles.Contains(x.Role)))
            .GetValueOrDefault();

        return isAuthorized;
    }
}