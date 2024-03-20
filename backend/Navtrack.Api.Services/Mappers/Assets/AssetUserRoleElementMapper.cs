using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetUserRoleElementMapper
{
    public static AssetUserRoleElement Map(ObjectId userId, AssetRoleType assetRoleType)
    {
        return new AssetUserRoleElement
        {
            Role = assetRoleType,
            UserId = userId,
            CreatedDate = DateTime.UtcNow
        };
    }
}