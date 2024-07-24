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

    public float? Speed { get; set; }
    public float? Heading { get; set; }
    public float? Altitude { get; set; }
    public int? Satellites { get; set; }
    public float? HDOP { get; set; }
    public bool? Valid { get; set; }
    public GsmModel? Gsm { get; set; }
    public double? Odometer { get; set; }
}