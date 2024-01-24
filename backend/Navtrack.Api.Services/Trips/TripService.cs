using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Mappers.Locations;
using Navtrack.Api.Services.Mappers.Trips;
using Navtrack.Api.Services.Util;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Model.New;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Trips;

[Service(typeof(ITripService))]
public class TripService(IPositionRepository repository) : ITripService
{
    public async Task<TripListModel> GetTrips(string assetId, TripFilterModel tripFilter)
    {
        IEnumerable<TripModel> trips = await GetInternalTrips(assetId, tripFilter);

        TripListModel list = TripListMapper.Map(trips);

        return list;
    }

    private async Task<IEnumerable<TripModel>> GetInternalTrips(string assetId, TripFilterModel tripFilter)
    {
        List<PositionModel> locations = await GetLocations(assetId, tripFilter);

        List<TripModel> trips = CreateTrips(locations);

        trips = ApplyFiltering(trips, tripFilter);
        trips = ApplyOrdering(trips);

        return trips;
    }

    private static List<TripModel> CreateTrips(IEnumerable<PositionModel> source)
    {
        List<TripModel> trips = [];

        TripModel? currentTrip = null;
        PositionModel? lastLocation = null;

        foreach (PositionModel locationDocument in source)
        {
            if (lastLocation == null ||
                locationDocument.DateTime > lastLocation.DateTime.AddMinutes(5) ||
                DistanceCalculator.CalculateDistance(new Coordinates(lastLocation.Latitude, lastLocation.Longitude),
                    new Coordinates(locationDocument.Latitude, locationDocument.Longitude)) > 1000)
            {
                currentTrip = new TripModel();
                trips.Add(currentTrip);
            }

            currentTrip.Locations.Add(locationDocument);
            lastLocation = locationDocument;
        }

        AddDistance(trips);

        return trips;
    }

    private static void AddDistance(List<TripModel> trips)
    {
        foreach (TripModel trip in trips)
        {
            trip.Distance = TripDistanceCalculator.CalculateDistance(trip.Locations
                .Select(x => (new Coordinates(x.Latitude, x.Longitude), x.Odometer)).ToList());
        }
    }

    private async Task<List<PositionModel>> GetLocations(string assetId, DateFilter dateFilter)
    {
        DateFilter filter = new()
        {
            StartDate = dateFilter.StartDate?.AddHours(-12) ?? dateFilter.StartDate,
            EndDate = dateFilter.EndDate?.AddHours(12) ?? dateFilter.EndDate
        };

        List<PositionGroupDocument> locations = await repository.GetLocations(assetId, filter);

        List<PositionModel> mapped = locations.SelectMany(x => x.Positions).OrderBy(x => x.Date).Select(LocationMapper.Map).ToList();

        return mapped;
    }

    private static List<TripModel> ApplyFiltering(IEnumerable<TripModel> trips, TripFilterModel tripFilter)
    {
        List<TripModel> filteredTrips = trips.ToList();

        filteredTrips = ApplyDistanceFilter(filteredTrips);
        filteredTrips = ApplyDateFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyAvgAltitudeFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyAvgSpeedFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyDurationFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyGeofenceFilter(tripFilter, filteredTrips);

        return filteredTrips;
    }

    private static List<TripModel> ApplyDistanceFilter(List<TripModel> filteredTrips)
    {
        filteredTrips = filteredTrips
            .Where(x => x.Distance > 0)
            .ToList();

        return filteredTrips;
    }

    private static List<TripModel> ApplyGeofenceFilter(TripFilterModel tripFilter, List<TripModel> filteredTrips)
    {
        if (tripFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            filteredTrips = filteredTrips
                .Where(x => x.Locations
                    .Any(y => DistanceCalculator.IsInRadius(
                        new Coordinates(y.Latitude, y.Longitude),
                        new Coordinates(tripFilter.Latitude.Value, tripFilter.Longitude.Value),
                        tripFilter.Radius.Value)))
                .ToList();
        }

        return filteredTrips;
    }

    private static List<TripModel> ApplyDurationFilter(TripFilterModel tripFilter, List<TripModel> filteredTrips)
    {
        if (tripFilter.MinDuration.HasValue)
        {
            filteredTrips = filteredTrips.Where(x => x.Duration >= tripFilter.MinDuration.Value)
                .ToList();
        }

        if (tripFilter.MaxDuration.HasValue)
        {
            filteredTrips = filteredTrips.Where(x => x.Duration <= tripFilter.MaxDuration.Value)
                .ToList();
        }

        return filteredTrips;
    }

    private static List<TripModel> ApplyAvgSpeedFilter(TripFilterModel tripFilter, List<TripModel> filteredTrips)
    {
        if (tripFilter.MinAvgSpeed.HasValue)
        {
            filteredTrips = filteredTrips.Where(x => x.AverageSpeed >= tripFilter.MinAvgSpeed.Value)
                .ToList();
        }

        if (tripFilter.MaxAvgSpeed.HasValue)
        {
            filteredTrips = filteredTrips.Where(x => x.AverageSpeed <= tripFilter.MaxAvgSpeed.Value)
                .ToList();
        }

        return filteredTrips;
    }

    private static List<TripModel> ApplyAvgAltitudeFilter(TripFilterModel tripFilter, List<TripModel> filteredTrips)
    {
        if (tripFilter.MaxAvgAltitude.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.AverageAltitude <= tripFilter.MaxAvgAltitude.Value).ToList();
        }

        if (tripFilter.MinAvgAltitude.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.AverageAltitude >= tripFilter.MinAvgAltitude.Value).ToList();
        }

        return filteredTrips;
    }

    private static List<TripModel> ApplyDateFilter(DateFilter dateFilter, List<TripModel> filteredTrips)
    {
        if (dateFilter.StartDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartPosition.DateTime.Date >= dateFilter.StartDate.Value.Date).ToList();
        }

        if (dateFilter.EndDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartPosition.DateTime.Date <= dateFilter.EndDate.Value.Date).ToList();
        }

        return filteredTrips;
    }

    private static List<TripModel> ApplyOrdering(IEnumerable<TripModel> trips)
    {
        return trips.OrderBy(x => x.StartPosition.DateTime).ToList();
    }
}