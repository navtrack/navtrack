using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Navtrack.Library.Util
{
    public static class DistanceCalculator
    {
        public static int CalculateDistance(List<(decimal Latitude, decimal Longitude, double? Odometer)> locations)
        {
            int distance = 0;

            (_, _, double? firstOdometer) = locations.First();
            (_, _, double? lastOdometer) = locations.Last();

            if (firstOdometer.HasValue && lastOdometer.HasValue)
            {
                distance = (int) (lastOdometer.Value - firstOdometer.Value);
            }
            else
            {
                for (int i = 0; i < locations.Count - 1; i++)
                {
                    int locationToLocationDistance =
                        CalculateDistance((locations[i].Latitude, locations[i].Longitude, locations[i].Odometer),
                            (locations[i + 1].Latitude, locations[i + 1].Longitude, locations[i + 1].Odometer));

                    distance += locationToLocationDistance;
                }
            }

            return distance;
        }

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        public static int CalculateDistance((decimal latitude, decimal longitude, double? odometer) fromLocation,
            (decimal latitude, decimal longitude, double? odometer) toLocation)
        {
            if (toLocation.odometer.HasValue && fromLocation.odometer.HasValue)
            {
                return (int) (toLocation.odometer.Value - fromLocation.odometer.Value);
            }
            
            decimal d1 = fromLocation.latitude * (decimal) (Math.PI / 180.0);
            decimal num1 = fromLocation.longitude * (decimal) (Math.PI / 180.0);
            decimal d2 = toLocation.latitude * (decimal) (Math.PI / 180.0);
            decimal num2 = toLocation.longitude * (decimal) (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((double) ((d2 - d1) / (decimal) 2.0)), 2.0) +
                        Math.Cos((double) d1) * Math.Cos((double) d2) *
                        Math.Pow(Math.Sin((double) (num2 / (decimal) 2.0)), 2.0);

            return (int) (6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }
    }
}