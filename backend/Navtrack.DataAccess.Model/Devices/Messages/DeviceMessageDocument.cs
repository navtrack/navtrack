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
    }
    
    [BsonElement("mp")]
    public MessagePriority? MessagePriority { get; set; }
    
    [BsonElement("md")]
    public MessageMetadataElement Metadata { get; set; }
    
    [BsonElement("cid")]
    public ObjectId? ConnectionId { get; set; }

    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("p")]
    public PositionElement Position { get; set; }
    
    [BsonElement("d")]
    public DeviceElement? Device { get; set; }

    [BsonElement("v")]
    public VehicleElement? Vehicle { get; set; }
    
    [BsonElement("gsm")]
    public GsmElement? Gsm { get; set; }
    
    [BsonElement("ad")]
    public Dictionary<string, string> AdditionalData { get; set; }
    
    [BsonElement("adu")]
    public Dictionary<string, string>? AdditionalDataUnhandled { get; set; }

    // [BsonElement("s")]
    // public StatusElement Status { get; set; }

    // [BsonElement("t")]
    // public TripElement Trip { get; set; }

    // [BsonElement("io")]
    // public IOElement? IO { get; set; }
    
    // [BsonElement("can")]
    // public CanElement? Can { get; set; }
}