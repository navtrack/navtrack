using System;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Models;

public class Location
{
    public Location()
    {
    }

    public Location(GPRMC gprmc)
    {
        DateTime = gprmc.DateTime;
        Latitude = gprmc.Latitude;
        Longitude = gprmc.Longitude;
        PositionStatus = gprmc.PositionStatus;
        Speed = gprmc.Speed;
        Heading = gprmc.Heading;
    }

    public Device Device { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime DateTime { get; set; }
    public bool? PositionStatus { get; set; }
    public float? Speed { get; set; }
    public float? Heading { get; set; }
    public float? Altitude { get; set; }
    public short? Satellites { get; set; }
    public float? HDOP { get; set; }
    public short? GsmSignal { get; set; }
    public double? Odometer { get; set; }
        
    public int? MobileCountryCode { get; set; }
    public int? MobileNetworkCode { get; set; }
    public int? LocationAreaCode { get; set; }
    public int? CellId { get; set; }
}