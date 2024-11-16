namespace Navtrack.Api.Model.Messages;

public class MessageDevice
{
    public int? Odometer { get; set; }
    public byte? BatteryLevel { get; set; }
    public double? BatteryVoltage { get; set; }
    public double? BatteryCurrent { get; set; }
}