using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Positions;

[Collection("devices_messages")]
public class MessageDocument : BaseDocument
{
    [BsonElement("md")]
    public MessageMetadataElement Metadata { get; set; }

    [BsonElement("cId")]
    public ObjectId? ConnectionId { get; set; }

    [BsonElement("d")]
    public DateTime Date { get; set; }

    [BsonElement("p")]
    public PositionElement Position { get; set; }

    [BsonElement("gsm")]
    public GsmElement? Gsm { get; set; }
}