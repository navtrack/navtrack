using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class TripElement
{
    [BsonElement("es")]
    public double EcoScore { get; set; }
    
    [BsonElement("odo")]
    public uint Odometer { get; set; }

    [BsonElement("s")]
    public byte Status { get; set; }
}