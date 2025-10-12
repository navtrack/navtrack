using System;
using Navtrack.Api.Model.Assets;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetEntityMapper
{
    public static AssetEntity Map(Guid organizationId, CreateAssetModel source, Guid currentUserId)
    {
        return new AssetEntity
        {
            OrganizationId = organizationId,
            Name = source.Name,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId
        };
    }
}