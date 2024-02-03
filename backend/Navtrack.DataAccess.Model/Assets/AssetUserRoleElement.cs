using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Assets;

public class AssetUserRoleElement
{
    [BsonElement("userId")]
    public ObjectId UserId { get; set; }
        
    [BsonElement("role")]
    [BsonRepresentation(BsonType.String)]
    public AssetRoleType Role { get; set; }
}