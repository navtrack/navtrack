using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Messages;

public class LatLongModel(double latitude, double longitude)
{
    [Required]
    public double Latitude { get; set; } = latitude;

    [Required]
    public double Longitude { get; set; } = longitude;

    public bool Valid => CoordinatesValidation.IsValidLatitude(Latitude) &&
                         CoordinatesValidation.IsValidLongitude(Longitude);
}