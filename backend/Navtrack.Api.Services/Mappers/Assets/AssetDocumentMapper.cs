using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetDocumentMapper
{
    public static AssetDocument Map(CreateAssetModel source, UserDocument owner)
    {
        return new AssetDocument
        {
            Id = ObjectId.GenerateNewId(),
            Name = source.Name,
            OwnerId = owner.Id,
            UserRoles = new List<AssetUserRoleElement>
            {
                AssetUserRoleElementMapper.Map(owner.Id, AssetRoleType.Owner)
            },
            CreatedDate = DateTime.UtcNow,
            CreatedBy = owner.Id
        };
    }
}