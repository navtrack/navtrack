using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class DeviceElement
{
    /// <summary>
    /// In meters.
    /// </summary>
    [BsonElement("odo")]
    public int? Odometer { get; set; }

    /// <summary>
    /// Battery level (0%-100%)
    /// </summary>
    [BsonElement("bl")]
    public byte? BatteryLevel { get; set; }

    /// <summary>
    /// Battery voltage (V)
    /// </summary>
    [BsonElement("bv")]
    public double? BatteryVoltage { get; set; }

    /// <summary>
    /// Battery current (A)
    /// </summary>
    [BsonElement("bc")]
    public double? BatteryCurrent { get; set; }

    public bool IsNull()
    {
        return Odometer == null && BatteryLevel == null && BatteryVoltage == null && BatteryCurrent == null;
    }
}