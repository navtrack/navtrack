using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Positions;

public class PositionModel
{
    public string Id { get; set; }
    
    /// <summary>
    /// [Longitude, Latitude]
    /// </summary>
    [Required]
    public LatLongModel Coordinates { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public double? Speed { get; set; }
    public double? Heading { get; set; }
    public double? Altitude { get; set; }
    public int? Satellites { get; set; }
    public double? HDOP { get; set; }
    public bool? Valid { get; set; }
    public GsmModel? Gsm { get; set; }
    public double? Odometer { get; set; }
}