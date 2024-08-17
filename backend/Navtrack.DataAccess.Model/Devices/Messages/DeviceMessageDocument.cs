using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Devices.Messages;

[Collection("devices_messages")]
public class DeviceMessageDocument : BaseDocument
{
    public DeviceMessageDocument()
    {
        Data = new Dictionary<string, string>();
        Extra = new Dictionary<string, string>();
    }
    
    [BsonElement("md")]
    public MessageMetadataElement Metadata { get; set; }

    [BsonElement("cId")]
    public ObjectId? ConnectionId { get; set; }

    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("p")]
    public PositionElement Position { get; set; }

    [BsonElement("gsm")]
    public GsmElement? Gsm { get; set; }

    [BsonElement("obd")]
    public ObdElement? Obd { get; set; }

    [BsonElement("ev")]
    public EventElement? Event { get; set; }

    [BsonElement("d")]
    public Dictionary<string, string>? Data { get; set; }
    
    [BsonElement("e")]
    public Dictionary<string, string>? Extra { get; set; }
}