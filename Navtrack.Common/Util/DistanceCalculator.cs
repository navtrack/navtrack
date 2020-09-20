using System;
using System.Collections.Generic;

namespace Navtrack.Common.Util
{
    public static class DistanceCalculator
    {
        public static double GetDistance(IReadOnlyList<(decimal latitude, decimal longitude)> locations)
        {
            double sum = 0;
            
            for (int i = 0; i < locations.Count - 1; i++)
            {
                double distance =
                    CalculateDistance((locations[i].latitude, locations[i].longitude),
                        (locations[i + 1].latitude, locations[i + 1].longitude));

                sum += distance;
            }

            return sum;
        }
        
        public static double CalculateDistance((decimal latitude, decimal longitude) position1,
            (decimal latitude, decimal longitude) position2)
        {
            decimal d1 = position1.latitude * (decimal) (Math.PI / 180.0);
            decimal num1 = position1.longitude * (decimal) (Math.PI / 180.0);
            decimal d2 = position2.latitude * (decimal) (Math.PI / 180.0);
            decimal num2 = position2.longitude * (decimal) (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((double) ((d2 - d1) / (decimal) 2.0)), 2.0) +
                        Math.Cos((double) d1) * Math.Cos((double) d2) *
                        Math.Pow(Math.Sin((double) (num2 / (decimal) 2.0)), 2.0);
            
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}