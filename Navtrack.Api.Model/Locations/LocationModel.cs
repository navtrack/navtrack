using System;
using System.ComponentModel.DataAnnotations;
using Navtrack.Core.Services.Util;

namespace Navtrack.Api.Model.Locations;

public class LocationModel
{
    public string Id { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required]
    public double[] Coordinates { get; set; }

    [Required]
    public bool ValidCoordinates => CoordinatesValidation.IsValidLatitude(Latitude) &&
                                    CoordinatesValidation.IsValidLongitude(Longitude);

    [Required]
    public DateTime DateTime { get; set; }

    public float? Speed { get; set; }
    public float? Heading { get; set; }
    public float? Altitude { get; set; }
    public int? Satellites { get; set; }
    public float? HDOP { get; set; }
    public bool? Valid { get; set; }
    public short? GsmSignal { get; set; }
    public double? Odometer { get; set; }
}