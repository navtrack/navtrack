using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Attributes;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model.Devices;

[Collection("devices")]
public class DeviceDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("serialNumber")]
    public string SerialNumber { get; set; }

    [BsonElement("deviceTypeId")]
    public int DeviceTypeId { get; set; }

    [BsonElement("protocolPort")]
    public int ProtocolPort { get; set; }

    [BsonElement("assetId")]
    public ObjectId AssetId { get; set; }

    [BsonElement("created")]
    public AuditElement Created { get; set; }
}