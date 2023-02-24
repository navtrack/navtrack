using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Trips;
using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.Api.Services.Mappers;

public static class TripListMapper
{
    public static TripListModel Map(IEnumerable<Core.Model.Trips.Trip> source, UnitsType unitsType)
    {
        return new TripListModel
        {
            Items = source.Select(trip => new TripModel
            {
                Locations = trip.Locations.Select(y => LocationMapper.Map(y, unitsType)).ToList(),
                Distance = trip.Distance,
                Duration = trip.Duration,
                MaxSpeed = trip.MaxSpeed,
                AverageSpeed = trip.AverageSpeed,
                AverageAltitude = trip.AverageAltitude
            }).ToList()
        };

        // TripList list = new()
        // {
        //     Items = trips.Select(trip =>
        //         {
        //             trip.DistanceByDay = new Dictionary<int, int>();
        //             trip.DurationByDay = new Dictionary<int, double>();
        //             
        //             for (int i = 0; i < trip.Locations.Count - 1; i++)
        //             {
        //                 var from = trip.Locations[i];
        //                 var to = trip.Locations[i + 1];
        //
        //                 var distance = TripDistanceCalculator.CalculateDistance(
        //                     (from.Latitude, from.Longitude, from.Odometer),
        //                     (to.Latitude, to.Longitude, to.Odometer));
        //
        //                 var time = Math.Ceiling((to.DateTime - from.DateTime).TotalSeconds);
        //
        //                 if (trip.DistanceByDay.ContainsKey(to.DateTime.Day))
        //                 {
        //                     trip.DistanceByDay[to.DateTime.Day] += distance;
        //                 }
        //                 else
        //                 {
        //                     trip.DistanceByDay[to.DateTime.Day] = distance;
        //                 }
        //
        //                 if (trip.DurationByDay.ContainsKey(to.DateTime.Day))
        //                 {
        //                     trip.DurationByDay[to.DateTime.Day] += time;
        //                 }
        //                 else
        //                 {
        //                     trip.DurationByDay[to.DateTime.Day] = time;
        //                 }
        //             }
        //
        //             foreach (KeyValuePair<int,double> keyValuePair in trip.DurationByDay)
        //             {
        //                 trip.DurationByDay[keyValuePair.Key] = keyValuePair.Value / 60;
        //             }
        //
        //             trip.Distance = TripDistanceCalculator.CalculateDistance(trip.Locations.Select(x =>
        //                 (x.Latitude, x.Longitude, x.Odometer)).ToList());
        //
        //             return trip;
        //         })
        //         .Where(x => x.Distance > 0)
        //         .OrderByDescending(x => x.EndLocation.DateTime)
        //         .ToList()
        // };
    }
}