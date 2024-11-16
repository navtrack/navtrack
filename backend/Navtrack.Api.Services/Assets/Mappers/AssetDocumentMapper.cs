using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetDocumentMapper
{
    public static AssetDocument Map(CreateAssetRequest source, ObjectId userId)
    {
        return new AssetDocument
        {
            OrganizationId = ObjectId.Parse(source.OrganizationId),
            Name = source.Model.Name,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId
        };
    }
}