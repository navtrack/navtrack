namespace Navtrack.Core.Services.Util;

public static class CoordinatesValidation
{
    public static bool IsValidLatitude(double latitude) => latitude is >= -90 and <= 90;

    public static bool IsValidLongitude(double longitude) => longitude is >= -180 and <= 180;
}