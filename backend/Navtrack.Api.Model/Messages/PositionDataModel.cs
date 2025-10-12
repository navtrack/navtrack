using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Messages;

public class PositionDataModel
{
    /// <summary>
    /// [Longitude, Latitude]
    /// </summary>
    [Required]
    public LatLong Coordinates { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public double Speed { get; set; }
    public double Heading { get; set; }
    public double Altitude { get; set; }
    public int Satellites { get; set; }
    public double HDOP { get; set; }
    public double? PDOP { get; set; }
    public bool Valid { get; set; }
}