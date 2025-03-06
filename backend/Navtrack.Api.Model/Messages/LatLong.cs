using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Messages;

public class LatLong(double latitude, double longitude)
{
    [Required]
    public double Latitude { get; set; } = latitude;

    [Required]
    public double Longitude { get; set; } = longitude;
}