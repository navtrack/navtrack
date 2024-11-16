using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.DeviceMessages.Mappers;
using Navtrack.Api.Services.Trips.Mappers;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;
using DateFilter = Navtrack.DataAccess.Model.Devices.Messages.Filters.DateFilter;

namespace Navtrack.Api.Services.Trips;

[Service(typeof(ITripService))]
public class TripService(IDeviceMessageRepository repository) : ITripService
{
    private const int MinTripDistanceInMeters = 300; // 1000 feet
    private const int MaxDistanceBetweenPositionsInMeters = 1000;
    private const double MaxTimeBetweenTripInMinutes = 5;

    public async Task<TripList> GetTrips(string assetId, TripFilter tripFilter)
    {
        IEnumerable<Trip> trips = await GetInternalTrips(assetId, tripFilter);

        TripList list = TripListMapper.Map(trips);

        return list;
    }

    private async Task<IEnumerable<Trip>> GetInternalTrips(string assetId, TripFilter tripFilter)
    {
        List<MessagePosition> positions = await GetPositions(assetId, tripFilter);

        List<Trip> trips = CreateTrips(positions);

        trips = ApplyFiltering(trips, tripFilter);
        trips = ApplyOrdering(trips);

        return trips;
    }

    private static List<Trip> CreateTrips(List<MessagePosition> source)
    {
        List<Trip> trips = [];

        Trip? currentTrip = null;
        MessagePosition? lastPosition = null;

        foreach (MessagePosition position in source)
        {
            if (currentTrip == null ||
                lastPosition == null ||
                position.Date > lastPosition.Date.AddMinutes(MaxTimeBetweenTripInMinutes) ||
                DistanceCalculator.CalculateDistance(lastPosition.Coordinates, position.Coordinates) > MaxDistanceBetweenPositionsInMeters)
            {
                currentTrip = new Trip();
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

    private static void TrimPositionsEnds(List<Trip> trips)
    {
        foreach (Trip trip in trips)
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

    private static void RemoveDuplicatePositions(List<Trip> trips)
    {
        const double precision = 0.000001;

        foreach (Trip trip in trips)
        {
            List<MessagePosition> positions = [];

            MessagePosition? lastPosition = null;

            foreach (MessagePosition position in trip.Positions)
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

    private static void AddDistance(List<Trip> trips)
    {
        foreach (Trip trip in trips)
        {
            trip.Distance = DistanceCalculator.CalculateDistance(trip.Positions
                .Select(x => (x.Coordinates, (uint?)null)).ToList());
        }
    }

    private async Task<List<MessagePosition>> GetPositions(string assetId, DateFilter dateFilter)
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

        List<MessagePosition> mapped = result.Messages
            .Select(x => MessagePositionMapper.Map(x.Position))
            .ToList();

        return mapped;
    }

    private static List<Trip> ApplyFiltering(IEnumerable<Trip> trips, TripFilter tripFilter)
    {
        List<Trip> filteredTrips = trips.ToList();

        filteredTrips = ApplyDistanceFilter(filteredTrips);
        filteredTrips = ApplyDateFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyAvgAltitudeFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyAvgSpeedFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyDurationFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyGeofenceFilter(tripFilter, filteredTrips);

        return filteredTrips;
    }

    private static List<Trip> ApplyDistanceFilter(List<Trip> filteredTrips)
    {
        filteredTrips = filteredTrips
            .Where(x => x.Distance > MinTripDistanceInMeters)
            .ToList();

        return filteredTrips;
    }

    private static List<Trip> ApplyGeofenceFilter(TripFilter tripFilter, List<Trip> filteredTrips)
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

    private static List<Trip> ApplyDurationFilter(TripFilter tripFilter, List<Trip> filteredTrips)
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

    private static List<Trip> ApplyAvgSpeedFilter(TripFilter tripFilter, List<Trip> filteredTrips)
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

    private static List<Trip> ApplyAvgAltitudeFilter(TripFilter tripFilter, List<Trip> filteredTrips)
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

    private static List<Trip> ApplyDateFilter(DateFilter dateFilter, List<Trip> filteredTrips)
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

    private static List<Trip> ApplyOrdering(IEnumerable<Trip> trips)
    {
        return trips.OrderBy(x => x.StartPosition.Date).ToList();
    }
}