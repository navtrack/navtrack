using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Shared;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Devices;

[Collection("devices")]
public class DeviceDocument : UpdatedAuditDocument
{
    [BsonElement("serialNumber")]
    public string SerialNumber { get; set; }

    [BsonElement("deviceTypeId")]
    public string DeviceTypeId { get; set; }
    
    [BsonElement("organizationId")]
    public ObjectId OrganizationId { get; set; }

    [BsonElement("assetId")]
    public ObjectId AssetId { get; set; }
}