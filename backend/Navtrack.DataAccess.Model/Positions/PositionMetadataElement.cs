using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Positions;

public class PositionMetadataElement
{
    [BsonElement("aId")]
    public ObjectId AssetId { get; set; }

    [BsonElement("dId")]
    public ObjectId DeviceId { get; set; }
}