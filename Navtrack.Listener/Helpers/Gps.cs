namespace Navtrack.Listener.Helpers
{
    public static class Gps
    {
        public static bool IsValidLatitude(decimal latitude) => latitude >= -90 && latitude <= 90;

        public static bool IsValidLongitude(decimal longitude) => longitude >= -180 && longitude <= 180;
    }
}