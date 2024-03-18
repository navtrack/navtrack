using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class MessageMetadataElement
{
    [BsonElement("aId")]
    public ObjectId AssetId { get; set; }

    [BsonElement("dId")]
    public ObjectId DeviceId { get; set; }
}