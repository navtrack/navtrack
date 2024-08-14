namespace Navtrack.Listener.Protocols.Teltonika;

internal class TeltonikaPositionOEMOBDElements
{
    public uint? TotalMileage { get; set; }
    public uint? FuelLevel { get; set; }
    public uint? DistanceUntilService { get; set; }
    public byte? BatteryChargeState { get; set; }
    public byte? BatteryChargeLevel { get; set; }
    public ushort? BatteryPowerConsumption { get; set; }
    public ushort? RemainingDistance { get; set; }
    public ushort? BatteryStateOfHealth { get; set; }
    public short? BatteryTemperature { get; set; }
}