using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Shared;

namespace Navtrack.DataAccess.Model.Users;

public class UserAssetElement : UpdatedAuditElement
{
    [BsonElement("assetId")]
    public ObjectId AssetId { get; set; }

    [BsonElement("organizationId")]
    public ObjectId OrganizationId { get; set; }

    [BsonElement("role")]
    [BsonRepresentation(BsonType.String)]
    public AssetUserRole UserRole { get; set; }
}