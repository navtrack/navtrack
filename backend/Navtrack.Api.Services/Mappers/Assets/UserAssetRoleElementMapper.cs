using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class UserAssetRoleElementMapper
{
    public static UserAssetRoleElement Map(ObjectId assetId, AssetRoleType assetRoleType)
    {
        return new UserAssetRoleElement 
        {
            AssetId = assetId,
            Role = assetRoleType,
            CreatedDate = DateTime.UtcNow
        };
    }
}