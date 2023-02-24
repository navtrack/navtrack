using System.Collections.Generic;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Model.Trips;
using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Api.Services.Mappers;

public static class DistanceReportMapper
{
    public static DistanceReportListModel Map(IEnumerable<LocationDocument> source, UnitsType unitsType)
    {
        List<TripModel> trips = new();
        TripModel currentTripModel = null;
        LocationDocument lastLocation = null;

        foreach (LocationDocument locationDocument in source)
        {
            if (lastLocation == null || locationDocument.DateTime > lastLocation.DateTime.AddMinutes(5))
            {
                currentTripModel = new TripModel
                {
                    Locations = new List<LocationModel>()
                };
                trips.Add(currentTripModel);
            }

            currentTripModel.Locations.Add(LocationMapper.Map(locationDocument, unitsType));

            lastLocation = locationDocument;
        }

        DistanceReportListModel listModel = new()
        {
            // Items = trips.Select(trip =>
            //     {
            //         trip.Distance = TripDistanceCalculator.CalculateDistance(trip.Locations.Select(x =>
            //             (x.Latitude, x.Longitude, x.Odometer)).ToList());
            //
            //         return trip;
            //     })
            //     .Where(x => x.Distance > 0)
            //     .OrderByDescending(x => x.EndLocation.DateTime)
            //     .ToList()
        };

        return listModel;
    }
}