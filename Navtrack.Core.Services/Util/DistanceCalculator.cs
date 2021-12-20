using System;
using Navtrack.Core.Model;

namespace Navtrack.Core.Services.Util;

public static class DistanceCalculator
{
    public static int CalculateDistance(Coordinates source, Coordinates destination)
    {
        double d1 = source.Latitude * (Math.PI / 180.0);
        double num1 = source.Longitude * (Math.PI / 180.0);
        double d2 = destination.Latitude * (Math.PI / 180.0);
        double num2 = destination.Longitude * (Math.PI / 180.0) - num1;
        double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                    Math.Cos(d1) * Math.Cos(d2) *
                    Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return (int)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
    }

    public static bool IsInRadius(Coordinates origin, Coordinates target, int radius)
    {
        int distance = CalculateDistance(origin, target);

        return distance <= radius;
    }
}