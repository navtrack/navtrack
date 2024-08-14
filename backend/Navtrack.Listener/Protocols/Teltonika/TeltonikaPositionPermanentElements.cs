namespace Navtrack.Listener.Protocols.Teltonika;

internal class TeltonikaPositionPermanentElements
{
    public bool? Ignition { get; set; }
    /// <summary>
    /// Logic: 0/1
    /// </summary>
    public bool? DigitalInput1 { get; set; }
    public ulong? ICCID1 { get; set; }
    public ulong? ICCID2 { get; set; }
    
    /// <summary>
    /// Total Odometer value in meters
    /// </summary>
    public int? TotalOdometer { get; set; }
    public short? AxisX { get; set; }
    
    /// <summary>
    /// Voltage (V)
    /// </summary>
    public double? ExternalVoltage { get; set; }
    
    /// <summary>
    /// 0 - GNSS OFF
    /// 1 – GNSS ON with fix
    /// 2 - GNSS ON without fix
    /// 3 - GNSS sleep
    /// </summary>
    public byte? GNSSStatus { get; set; }
    public double? GNSSHDOP { get; set; }
    public double? GNSSPDOP { get; set; }
    public uint? TripOdometer { get; set; }
    public bool? Movement { get; set; }
    public uint? ActiveGSMOperator { get; set; }
    public byte? SleepMode { get; set; }
    public double? EcoScore { get; set; }
    
    /// <summary>
    /// Value in range 1-5
    /// </summary>
    public byte? GSMSignal { get; set; }

    public ushort? Speed { get; set; }
}