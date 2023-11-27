using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Devices;

[Collection("devices")]
[BsonIgnoreExtraElements]
public class DeviceDocument : BaseDocument
{
    [BsonElement("serialNumber")]
    public string SerialNumber { get; set; }

    [BsonElement("deviceTypeId")]
    public string DeviceTypeId { get; set; }

    [BsonElement("assetId")]
    public ObjectId AssetId { get; set; }

    [BsonElement("created")]
    public AuditElement Created { get; set; }
}