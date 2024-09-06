using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class MessageMetadataElement
{
    [BsonElement("aid")]
    public ObjectId AssetId { get; set; }

    [BsonElement("did")]
    public ObjectId DeviceId { get; set; }
}