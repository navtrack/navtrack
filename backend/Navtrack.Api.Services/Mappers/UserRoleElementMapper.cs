using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Mappers;

public static class UserRoleElementMapper
{
    public static AssetUserRoleElement Map(ObjectId userId, AssetRoleType assetRoleType)
    {
        return new AssetUserRoleElement
        {
            Id = ObjectId.GenerateNewId(),
            Role = assetRoleType,
            UserId = userId
        };
    }
}