using System.Collections.Generic;
using System.Linq;
using Navtrack.Core.Model;

namespace Navtrack.Core.Services.Util;

public static class TripDistanceCalculator
{
    public static int CalculateDistance(List<(Coordinates Coordinates, double? Odometer)> locations)
    {
        int distance = 0;

        (_, double? firstOdometer) = locations.First();
        (_, double? lastOdometer) = locations.Last();

        if (firstOdometer.HasValue && lastOdometer.HasValue)
        {
            distance = (int)(lastOdometer.Value - firstOdometer.Value);
        }
        else
        {
            for (int i = 0; i < locations.Count - 1; i++)
            {
                int locationToLocationDistance =
                    CalculateDistance((locations[i].Coordinates, locations[i].Odometer),
                        (locations[i + 1].Coordinates, locations[i + 1].Odometer));

                distance += locationToLocationDistance;
            }
        }

        return distance;
    }

    public static int CalculateDistance((Coordinates Coordinates, double? Odometer) fromLocation,
        (Coordinates Coordinates, double? Odometer) toLocation)
    {
        if (toLocation.Odometer.HasValue && fromLocation.Odometer.HasValue)
        {
            return (int)(toLocation.Odometer.Value - fromLocation.Odometer.Value);
        }

        return DistanceCalculator.CalculateDistance(fromLocation.Coordinates, toLocation.Coordinates);
    }
}