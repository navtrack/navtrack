using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class UserAssetElementMapper
{
    public static UserAssetElement Map(ObjectId assetId, AssetUserRole userRole, ObjectId organizationId)
    {
        return new UserAssetElement 
        {
            AssetId = assetId,
            UserRole = userRole,
            CreatedDate = DateTime.UtcNow,
            OrganizationId = organizationId
        };
    }
}