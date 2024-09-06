using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Services.Trips;

public static class DistanceCalculator
{
    /// <summary>
    /// Calculates the distance between two coordinates.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>The distance in meters.</returns>
    public static int CalculateDistance(LatLongModel from, LatLongModel to)
    {
        double d1 = from.Latitude * (Math.PI / 180.0);
        double num1 = from.Longitude * (Math.PI / 180.0);
        double d2 = to.Latitude * (Math.PI / 180.0);
        double num2 = to.Longitude * (Math.PI / 180.0) - num1;
        double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                    Math.Cos(d1) * Math.Cos(d2) *
                    Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return (int)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
    }

    public static bool IsInRadius(LatLongModel position, LatLongModel center, int radius)
    {
        int distance = CalculateDistance(position, center);

        return distance <= radius;
    }
    
    public static int CalculateDistance(List<(LatLongModel Coordinates, uint? Odometer)> positions)
    {
        int distance = 0;

        (_, uint? firstOdometer) = positions.First();
        (_, uint? lastOdometer) = positions.Last();

        if (firstOdometer.HasValue && lastOdometer.HasValue)
        {
            distance = (int)(lastOdometer.Value - firstOdometer.Value);
        }
        else
        {
            for (int i = 0; i < positions.Count - 1; i++)
            {
                int locationToLocationDistance =
                    CalculateDistance((positions[i].Coordinates, positions[i].Odometer),
                        (positions[i + 1].Coordinates, positions[i + 1].Odometer));

                distance += locationToLocationDistance;
            }
        }

        return distance;
    }

    private static int CalculateDistance((LatLongModel Coordinates, double? Odometer) from,
        (LatLongModel Coordinates, double? Odometer) to)
    {
        if (to.Odometer.HasValue && from.Odometer.HasValue)
        {
            return (int)(to.Odometer.Value - from.Odometer.Value);
        }

        return CalculateDistance(from.Coordinates, to.Coordinates);
    }
}