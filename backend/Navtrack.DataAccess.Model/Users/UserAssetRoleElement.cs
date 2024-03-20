using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.DataAccess.Model.Users;

public class UserAssetRoleElement
{
    [BsonElement("assetId")]
    public ObjectId AssetId { get; set; }

    [BsonElement("role")]
    [BsonRepresentation(BsonType.String)]
    public AssetRoleType Role { get; set; }
    
    [BsonElement("createdDate")]
    public DateTime? CreatedDate { get; set; }
}