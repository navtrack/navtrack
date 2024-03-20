using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Positions;

public class PositionModel
{
    public string Id { get; set; }
    
    [Required]
    public double Longitude => Coordinates[0];
    
    [Required]
    public double Latitude => Coordinates[1];

    [Required]
    public double[] Coordinates { get; set; }

    [Required]
    public bool ValidCoordinates => CoordinatesValidation.IsValidLatitude(Latitude) &&
                                    CoordinatesValidation.IsValidLongitude(Longitude);

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