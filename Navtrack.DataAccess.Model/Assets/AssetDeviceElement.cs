using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Assets;

[BsonIgnoreExtraElements]
public class AssetDeviceElement
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("serialNumber")]
    public string SerialNumber { get; set; }

    [BsonElement("deviceTypeId")]
    public string DeviceTypeId { get; set; }

    [BsonElement("protocolPort")]
    public int ProtocolPort { get; set; }
}