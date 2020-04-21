using System;
using System.Text.RegularExpressions;

namespace Navtrack.Listener.Helpers
{
    public static class GpsUtil
    {
        public static bool IsValidLatitude(decimal latitude) => latitude >= -90 && latitude <= 90;

        public static bool IsValidLongitude(decimal longitude) => longitude >= -180 && longitude <= 180;
        
        public static decimal ConvertDegreeAngleToDouble(string regExPattern, string point, string cardinalDirection)
        {
            MatchCollection matchCollection = new Regex(regExPattern).Matches(point);

            int multiplier = cardinalDirection == "S" || cardinalDirection == "W" ? -1 : 1;
            decimal degrees = decimal.Parse(matchCollection[0].Groups[1].Value);
            decimal minutes = decimal.Parse(matchCollection[0].Groups[2].Value) / 60;
            decimal seconds = decimal.Parse(matchCollection[0].Groups[3].Value) / 3600;

            return Math.Round((degrees + minutes + seconds) * multiplier, 6, MidpointRounding.ToZero);
        }
    }
}