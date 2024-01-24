using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.New;

[Collection("connections")]
public class ConnectionDocument : BaseDocument
{
    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("pp")]
    public int ProtocolPort { get; set; }

    [BsonElement("ip")]
    public string? RemoteEndpoint { get; set; }

    [BsonElement("m")]
    public List<string> Messages { get; set; }
}