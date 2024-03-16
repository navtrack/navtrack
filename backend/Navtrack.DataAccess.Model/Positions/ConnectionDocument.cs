using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Positions;

[Collection("device_connections")]
public class ConnectionDocument : BaseDocument
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