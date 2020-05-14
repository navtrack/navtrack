using System;
using System.Text.RegularExpressions;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Helpers
{
    public static class GpsUtil
    {
        public static bool IsValidLatitude(decimal latitude) => latitude >= -90 && latitude <= 90;

        public static bool IsValidLongitude(decimal longitude) => longitude >= -180 && longitude <= 180;

        public static decimal ConvertDmmLatToDecimal(string point, string cardinalDirection) =>
            ConvertDmmToDecimal(@"(\d{2})(\d{2}.\d+)", point, cardinalDirection);
        
        public static decimal ConvertDmmLongToDecimal(string point, string cardinalDirection) =>
            ConvertDmmToDecimal(@"(\d{3})(\d{2}.\d+)", point, cardinalDirection);

        public static decimal ConvertDmsToDecimal(string regExPattern, string point, string cardinalDirection)
        {
            MatchCollection matchCollection = new Regex(regExPattern).Matches(point);

            int multiplier = cardinalDirection == "S" || cardinalDirection == "W" ? -1 : 1;
            decimal degrees = decimal.Parse(matchCollection[0].Groups[1].Value);
            decimal minutes = decimal.Parse(matchCollection[0].Groups[2].Value) / 60;
            decimal seconds = decimal.Parse(matchCollection[0].Groups[3].Value) / 3600;

            return Math.Round((degrees + minutes + seconds) * multiplier, 6, MidpointRounding.ToZero);
        }

        public static decimal ConvertDmmToDecimal(string regExPattern, string point, string cardinalDirection)
        {
            MatchCollection matchCollection = new Regex(regExPattern).Matches(point);

            int multiplier = cardinalDirection == "S" || cardinalDirection == "W" ? -1 : 1;
            decimal degrees = decimal.Parse(matchCollection[0].Groups[1].Value);
            decimal minutes = decimal.Parse(matchCollection[0].Groups[2].Value) / 60;

            return Math.Round((degrees + minutes) * multiplier, 6, MidpointRounding.ToZero);
        }

        public static decimal ConvertDmmToDecimal(decimal degrees, decimal minutes, CardinalPoint cardinalPoint)
        {
            int multiplier = cardinalPoint == CardinalPoint.South || cardinalPoint == CardinalPoint.West ? -1 : 1;
          
            return Math.Round((degrees + minutes) * multiplier, 6, MidpointRounding.ToZero);
        }

        public static decimal ConvertDdmToDecimal(decimal degrees, decimal minutes, CardinalPoint cardinalPoint)
        {
            int multiplier = cardinalPoint == CardinalPoint.South || cardinalPoint == CardinalPoint.West ? -1 : 1;
          
            return Math.Round((degrees + minutes/60) * multiplier, 6, MidpointRounding.ToZero);
        }

        public static decimal ConvertStringToDecimal(string value, string cardinalDirection)
        {
            int multiplier = cardinalDirection == "S" || cardinalDirection == "W" ? -1 : 1;

            decimal result = decimal.Parse(value) * multiplier;

            return result;
        }
    }
}