using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Navtrack.Api.Model.Positions;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Mappers.Positions;
using Navtrack.Api.Services.Mappers.Trips;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Trips;

[Service(typeof(ITripService))]
public class TripService(IDeviceMessageRepository repository) : ITripService
{
    private const int MinTripDistanceInMeters = 300; // 1000 feet
    private const int MaxDistanceBetweenPositionsInMeters = 1000;
    private const double MaxTimeBetweenTripInMinutes = 5;

    public async Task<TripListModel> GetTrips(string assetId, TripFilterModel tripFilter)
    {
        IEnumerable<TripModel> trips = await GetInternalTrips(assetId, tripFilter);

        TripListModel list = TripListMapper.Map(trips);

        return list;
    }

    private async Task<IEnumerable<TripModel>> GetInternalTrips(string assetId, TripFilterModel tripFilter)
    {
        List<PositionModel> positions = await GetPositions(assetId, tripFilter);

        List<TripModel> trips = CreateTrips(positions);

        trips = ApplyFiltering(trips, tripFilter);
        trips = ApplyOrdering(trips);

        return trips;
    }

    private static List<TripModel> CreateTrips(List<PositionModel> source)
    {
        List<TripModel> trips = [];

        TripModel? currentTrip = null;
        PositionModel? lastPosition = null;

        foreach (PositionModel position in source)
        {
            if (currentTrip == null ||
                lastPosition == null ||
                position.Date > lastPosition.Date.AddMinutes(MaxTimeBetweenTripInMinutes) ||
                DistanceCalculator.CalculateDistance(lastPosition.Coordinates, position.Coordinates) > MaxDistanceBetweenPositionsInMeters)
            {
                currentTrip = new TripModel();
                trips.Add(currentTrip);
            }

            lastPosition = position;
            currentTrip.Positions.Add(position);
        }

        trips = trips.Where(x => x.Positions.Count > 10).ToList();

        RemoveDuplicatePositions(trips);
        TrimPositionsEnds(trips);
        AddDistance(trips);

        return trips;
    }

    private static void TrimPositionsEnds(List<TripModel> trips)
    {
        foreach (TripModel trip in trips)
        {
            int firstMovingPosition = trip.Positions.FindIndex(x => x.Speed > 0);
            int lastMovingPosition = trip.Positions.FindLastIndex(x => x.Speed > 0);

            int firstPositionIndex = firstMovingPosition > 1 ? firstMovingPosition - 1 : 0;
            int lastPositionIndex = lastMovingPosition != -1 && lastMovingPosition < trip.Positions.Count - 2
                ? lastMovingPosition + 1
                : trip.Positions.Count - 1;

            trip.Positions = trip.Positions.GetRange(firstPositionIndex, lastPositionIndex - firstPositionIndex + 1);
        }
    }

    private static void RemoveDuplicatePositions(List<TripModel> trips)
    {
        const double precision = 0.000001;

        foreach (TripModel trip in trips)
        {
            List<PositionModel> positions = [];

            PositionModel? lastPosition = null;

            foreach (PositionModel position in trip.Positions)
            {
                if (lastPosition == null || Math.Abs(lastPosition.Coordinates.Latitude - position.Coordinates.Latitude) > precision ||
                    Math.Abs(lastPosition.Coordinates.Longitude - position.Coordinates.Longitude) > precision)
                {
                    positions.Add(position);
                    lastPosition = position;
                }
            }

            trip.Positions = positions;
        }
    }

    private static void AddDistance(List<TripModel> trips)
    {
        foreach (TripModel trip in trips)
        {
            trip.Distance = DistanceCalculator.CalculateDistance(trip.Positions
                .Select(x => (x.Coordinates, x.Odometer)).ToList());
        }
    }

    private async Task<List<PositionModel>> GetPositions(string assetId, DateFilter dateFilter)
    {
        GetMessagesOptions options = new()
        {
            AssetId = assetId,
            PositionFilter = new PositionFilter
            {
                StartDate = dateFilter.StartDate?.AddHours(-12) ?? dateFilter.StartDate,
                EndDate = dateFilter.EndDate?.AddHours(12) ?? dateFilter.EndDate
            },
            OrderFunc = Builders<DeviceMessageDocument>.Sort.Ascending(x => x.Position.Date)
        };

        GetMessagesResult result = await repository.GetMessages(options);

        List<PositionModel> mapped = result.Messages
            .Select(PositionModelMapper.Map)
            .ToList();

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
            .Where(x => x.Distance > MinTripDistanceInMeters)
            .ToList();

        return filteredTrips;
    }

    private static List<TripModel> ApplyGeofenceFilter(TripFilterModel tripFilter, List<TripModel> filteredTrips)
    {
        if (tripFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            filteredTrips = filteredTrips
                .Where(x => x.Positions
                    .Any(y => DistanceCalculator.IsInRadius(
                        y.Coordinates,
                        new LatLongModel(tripFilter.Latitude.Value, tripFilter.Longitude.Value),
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
        int minAvgSpeed = tripFilter.MinAvgSpeed ?? 1;

        filteredTrips = filteredTrips.Where(x => x.AverageSpeed >= minAvgSpeed)
            .ToList();

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
                .Where(x => x.StartPosition.Date.Date >= dateFilter.StartDate.Value.Date).ToList();
        }

        if (dateFilter.EndDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartPosition.Date.Date <= dateFilter.EndDate.Value.Date).ToList();
        }

        return filteredTrips;
    }

    private static List<TripModel> ApplyOrdering(IEnumerable<TripModel> trips)
    {
        return trips.OrderBy(x => x.StartPosition.Date).ToList();
    }
}