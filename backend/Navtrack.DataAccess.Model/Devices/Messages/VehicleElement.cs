using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class VehicleElement
{
    /// <summary>
    /// In meters.
    /// </summary>
    [BsonElement("odo")]
    public uint? Odometer { get; set; }

    [BsonElement("ign")]
    public bool? Ignition { get; set; }
    
    [BsonElement("ignd")]
    public int? IgnitionDuration { get; set; }

    [BsonElement("fuelc")]
    public double FuelConsumed { get; set; }

    [BsonElement("v")]
    public double? Voltage { get; set; }
    
    public bool IsNull()
    {
        return Odometer == null && Ignition == null && Voltage == null;
    }
}