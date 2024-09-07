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
        AdditionalData = new Dictionary<string, string>();
        AdditionalDataUnhandled = new Dictionary<string, string>();
        AdditionalDataException = new Dictionary<string, string>();
    }
    
    [BsonElement("mp")]
    public MessagePriority? MessagePriority { get; set; }
    
    [BsonElement("md")]
    public MessageMetadataElement Metadata { get; set; }
    
    [BsonElement("cid")]
    public ObjectId? ConnectionId { get; set; }

    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("pos")]
    public PositionElement Position { get; set; }
    
    [BsonElement("dev")]
    public DeviceElement? Device { get; set; }

    [BsonElement("veh")]
    public VehicleElement? Vehicle { get; set; }
    
    [BsonElement("gsm")]
    public GsmElement? Gsm { get; set; }
    
    [BsonIgnore]
    public Dictionary<string, string>? AdditionalData { get; set; }
    
    [BsonIgnore]
    public Dictionary<string, string>? AdditionalDataUnhandled { get; set; }
    
    [BsonIgnore]
    public Dictionary<string, string>? AdditionalDataException { get; set; }
}