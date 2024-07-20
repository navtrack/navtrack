using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Devices.Messages;

[Collection("devices_messages")]
public class DeviceMessageDocument : BaseDocument
{
    [BsonElement("md")]
    public MessageMetadataElement? Metadata { get; set; }

    [BsonElement("cId")]
    public ObjectId? ConnectionId { get; set; }

    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("p")]
    public PositionElement? Position { get; set; }

    [BsonElement("gsm")]
    public GsmElement? Gsm { get; set; }
}