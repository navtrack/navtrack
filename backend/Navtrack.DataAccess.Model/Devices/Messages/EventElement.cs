using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class EventElement
{
    [BsonElement("p")]
    public byte Priority { get; set; }
}