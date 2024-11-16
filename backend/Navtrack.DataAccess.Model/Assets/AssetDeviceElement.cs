using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Shared;

namespace Navtrack.DataAccess.Model.Assets;

public class AssetDeviceElement : CreatedAuditElement
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