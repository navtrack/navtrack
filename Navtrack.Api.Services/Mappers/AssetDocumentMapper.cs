using System.Collections.Generic;
using MongoDB.Bson;
using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

public static class AssetDocumentMapper
{
    public static AssetDocument Map(AddAssetModel source, UserDocument owner)
    {
        return new AssetDocument
        {
            Id = ObjectId.GenerateNewId(),
            Name = source.Name,
            UserRoles = new List<AssetUserRoleElement>
            {
                UserRoleElementMapper.Map(owner.Id, AssetRoleType.Owner)
            },
            Created = AuditElementMapper.Map(owner.Id)
        };
    }
}