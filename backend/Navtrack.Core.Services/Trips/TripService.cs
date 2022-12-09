using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Core.Model;
using Navtrack.Core.Model.Trips;
using Navtrack.Core.Services.Util;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Services.Locations;
using Navtrack.Library.DI;

namespace Navtrack.Core.Services.Trips;

[Service(typeof(ITripService))]
public class TripService : ITripService
{
    private readonly ILocationDataService locationDataService;

    public TripService(ILocationDataService locationDataService)
    {
        this.locationDataService = locationDataService;
    }

    public async Task<IEnumerable<Trip>> GetTrips(string assetId, TripFilter tripFilter)
    {
        List<LocationDocument> locations = await GetLocations(assetId, tripFilter);

        IEnumerable<Trip> trips = TripsMapper.Map(locations);

        trips = ApplyFiltering(trips, tripFilter);
        trips = ApplyOrdering(trips);

        return trips;
    }

    private async Task<List<LocationDocument>> GetLocations(string assetId, DateFilter dateFilter)
    {
        DateFilter filter = new()
        {
            StartDate = dateFilter.StartDate?.AddDays(-1) ?? dateFilter.StartDate,
            EndDate = dateFilter.EndDate?.AddDays(1) ?? dateFilter.EndDate
        };

        List<LocationDocument> locations =
            await locationDataService.GetLocations(assetId, filter);

        return locations;
    }

    private static IEnumerable<Trip> ApplyFiltering(IEnumerable<Trip> items, TripFilter locationFilter)
    {
        List<Trip> filteredTrips = items.ToList();

        filteredTrips = ApplyDateFilter(locationFilter, filteredTrips);
        filteredTrips = ApplyAvgAltitudeFilter(locationFilter, filteredTrips);
        filteredTrips = ApplyAvgSpeedFilter(locationFilter, filteredTrips);
        filteredTrips = ApplyDurationFilter(locationFilter, filteredTrips);
        filteredTrips = ApplyGeofenceFilter(locationFilter, filteredTrips);
        filteredTrips = ApplyDistanceFilter(filteredTrips);

        return filteredTrips;
    }

    private static List<Trip> ApplyDistanceFilter(List<Trip> filteredTrips)
    {
        filteredTrips = filteredTrips
            .Where(x => x.Distance > 0)
            .ToList();
        return filteredTrips;
    }

    private static List<Trip> ApplyGeofenceFilter(TripFilter locationFilter, List<Trip> filteredTrips)
    {
        if (locationFilter.Latitude.HasValue && locationFilter.Longitude.HasValue && locationFilter.Radius.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.Locations
                    .Any(y => DistanceCalculator.IsInRadius(
                        new Coordinates(y.Latitude, y.Longitude),
                        new Coordinates(locationFilter.Latitude.Value, locationFilter.Longitude.Value),
                        locationFilter.Radius.Value)))
                .ToList();
        }

        return filteredTrips;
    }

    private static List<Trip> ApplyDurationFilter(TripFilter locationFilter, List<Trip> filteredTrips)
    {
        if (locationFilter.MinDuration.HasValue)
        {
            filteredTrips = filteredTrips.Where(x => x.Duration >= locationFilter.MinDuration.Value)
                .ToList();
        }

        if (locationFilter.MaxDuration.HasValue)
        {
            filteredTrips = filteredTrips.Where(x => x.Duration <= locationFilter.MaxDuration.Value)
                .ToList();
        }

        return filteredTrips;
    }

    private static List<Trip> ApplyAvgSpeedFilter(TripFilter locationFilter, List<Trip> filteredTrips)
    {
        if (locationFilter.MinAvgSpeed.HasValue)
        {
            filteredTrips = filteredTrips.Where(x => x.AverageSpeed >= locationFilter.MinAvgSpeed.Value)
                .ToList();
        }

        if (locationFilter.MaxAvgSpeed.HasValue)
        {
            filteredTrips = filteredTrips.Where(x => x.AverageSpeed <= locationFilter.MaxAvgSpeed.Value)
                .ToList();
        }

        return filteredTrips;
    }

    private static List<Trip> ApplyAvgAltitudeFilter(TripFilter locationFilter, List<Trip> filteredTrips)
    {
        if (locationFilter.MaxAvgAltitude.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.AverageAltitude <= locationFilter.MaxAvgAltitude.Value).ToList();
        }

        if (locationFilter.MinAvgAltitude.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.AverageAltitude >= locationFilter.MinAvgAltitude.Value).ToList();
        }

        return filteredTrips;
    }

    private static List<Trip> ApplyDateFilter(TripFilter locationFilter, List<Trip> filteredTrips)
    {
        if (locationFilter.StartDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartLocation.DateTime.Date >= locationFilter.StartDate.Value.Date).ToList();
        }

        if (locationFilter.EndDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartLocation.DateTime.Date <= locationFilter.EndDate.Value.Date).ToList();
        }

        return filteredTrips;
    }

    private static IEnumerable<Trip> ApplyOrdering(IEnumerable<Trip> trips)
    {
        return trips.OrderByDescending(x => x.StartLocation.DateTime).ToList();
    }
}