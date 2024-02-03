using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Mappers.Users;

public static class UserRoleElementMapper
{
    public static AssetUserRoleElement Map(ObjectId userId, AssetRoleType assetRoleType)
    {
        return new AssetUserRoleElement
        {
            Role = assetRoleType,
            UserId = userId
        };
    }
}