using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Devices;

[Collection("devices")]
public class DeviceDocument : BaseDocument
{
    [BsonElement("serialNumber")]
    public string SerialNumber { get; set; }

    [BsonElement("deviceTypeId")]
    public string DeviceTypeId { get; set; }

    [BsonElement("assetId")]
    public ObjectId AssetId { get; set; }

    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; }
        
    [BsonElement("createdBy")]
    public ObjectId CreatedBy { get; set; }
}