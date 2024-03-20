using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Devices.Connections;

[Collection("devices_connections")]
public class DeviceConnectionDocument : BaseDocument
{
    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("pp")]
    public int ProtocolPort { get; set; }

    [BsonElement("ip")]
    public string? Ip { get; set; }

    [BsonElement("m")]
    public List<BsonBinaryData>? Messages { get; set; }
}