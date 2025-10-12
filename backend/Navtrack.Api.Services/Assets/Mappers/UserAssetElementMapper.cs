using System;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class UserAssetElementMapper
{
    public static AssetUserEntity Map(Guid userId, Guid assetId, AssetUserRole userRole, Guid currentUserId)
    {
        return new AssetUserEntity
        {
            UserId = userId,
            AssetId = assetId,
            UserRole = userRole,
            CreatedBy = currentUserId,
            CreatedDate = DateTime.UtcNow
        };
    }
}