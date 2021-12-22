using System;
using System.Text.RegularExpressions;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Helpers;

public static class GpsUtil
{
    public static bool IsValidLatitude(double latitude) => latitude is >= -90 and <= 90;

    public static bool IsValidLongitude(double longitude) => longitude is >= -180 and <= 180;

    public static double ConvertDmmLatToDecimal(string point, string cardinalDirection) =>
        ConvertDmmToDecimal(@"(\d+)(\d\d.\d+)", point, cardinalDirection);
        
    public static double ConvertDmmLongToDecimal(string point, string cardinalDirection) =>
        ConvertDmmToDecimal(@"(\d+)(\d\d.\d+)", point, cardinalDirection);

    public static decimal ConvertDmsToDecimal(string regExPattern, string point, string cardinalDirection)
    {
        MatchCollection matchCollection = new Regex(regExPattern).Matches(point);

        int multiplier = cardinalDirection == "S" || cardinalDirection == "W" ? -1 : 1;
        decimal degrees = decimal.Parse(matchCollection[0].Groups[1].Value);
        decimal minutes = decimal.Parse(matchCollection[0].Groups[2].Value) / 60;
        decimal seconds = decimal.Parse(matchCollection[0].Groups[3].Value) / 3600;

        return Math.Round((degrees + minutes + seconds) * multiplier, 6, MidpointRounding.ToZero);
    }

    public static double ConvertDmmToDecimal(string regExPattern, string point, string cardinalDirection)
    {
        MatchCollection matchCollection = new Regex(regExPattern).Matches(point);

        int multiplier = cardinalDirection == "S" || cardinalDirection == "W" ? -1 : 1;
        double degrees = double.Parse(matchCollection[0].Groups[1].Value);
        double minutes = double.Parse(matchCollection[0].Groups[2].Value) / 60;

        return Math.Round((degrees + minutes) * multiplier, 6, MidpointRounding.ToZero);
    }

    public static double ConvertDmmToDecimal(double degrees, double minutes, CardinalPoint cardinalPoint)
    {
        int multiplier = cardinalPoint == CardinalPoint.South || cardinalPoint == CardinalPoint.West ? -1 : 1;
          
        return Math.Round((degrees + minutes) * multiplier, 6, MidpointRounding.ToZero);
    }

    public static double ConvertDdmToDecimal(double degrees, double minutes, CardinalPoint cardinalPoint)
    {
        int multiplier = cardinalPoint == CardinalPoint.South || cardinalPoint == CardinalPoint.West ? -1 : 1;
          
        return Math.Round((degrees + minutes/60) * multiplier, 6, MidpointRounding.ToZero);
    }

    public static double ConvertStringToDecimal(string value, string cardinalDirection)
    {
        int multiplier = cardinalDirection == "S" || cardinalDirection == "W" ? -1 : 1;

        double result = double.Parse(value) * multiplier;

        return result;
    }
}