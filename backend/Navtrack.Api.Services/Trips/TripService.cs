using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Trips.Mappers;
using Navtrack.Api.Services.Trips.Models;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Filters;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Trips;

[Service(typeof(ITripService))]
public class TripService(IDeviceMessageRepository repository) : ITripService
{
    private const int MinTripDistanceInMeters = 300; // 1000 feet
    private const int MaxDistanceBetweenPositionsInMeters = 1000;
    private const double MaxTimeBetweenTripInMinutes = 5;

    public async Task<TripListModel> GetTrips(AssetEntity asset, TripFilterModel tripFilter)
    {
        MapFilter(asset, tripFilter);
        
        List<InternalTrip> trips = await GetInternalTrips(asset.Id, tripFilter);

        TripListModel list = TripListMapper.Map(trips);

        return list;
    }

    private static void MapFilter(AssetEntity asset, TripFilterModel tripFilter)
    {
        if (tripFilter.StartDate == null || tripFilter.EndDate == null)
        {
            DateTime latestPositionDate = asset.LastPositionMessage?.Date ?? DateTime.UtcNow;
            
            tripFilter.StartDate = latestPositionDate.Date;
            tripFilter.EndDate = latestPositionDate.Date;
        }

        if (tripFilter.StartDate == tripFilter.EndDate)
        {
            tripFilter.EndDate = tripFilter.EndDate.Value.AddDays(1);
        }
    }

    private async Task<List<InternalTrip>> GetInternalTrips(Guid assetId, TripFilterModel tripFilter)
    {
        List<DeviceMessageEntity> messages = await GetMessages(assetId, tripFilter);

        List<InternalTrip> trips = CreateTrips(messages);

        trips = ApplyFiltering(trips, tripFilter);
        trips = ApplyOrdering(trips);

        return trips;
    }

    private static List<InternalTrip> CreateTrips(List<DeviceMessageEntity> source)
    {
        List<InternalTrip> trips = [];

        InternalTrip? currentTrip = null;
        DeviceMessageEntity lastMessage = null;

        foreach (DeviceMessageEntity message in source)
        {
            if (currentTrip == null ||
                lastMessage == null ||
                message.Date > lastMessage.Date.AddMinutes(MaxTimeBetweenTripInMinutes) ||
                DistanceCalculator.CalculateDistance(
                    new LatLong(lastMessage.Latitude, lastMessage.Longitude),
                    new LatLong(message.Latitude, lastMessage.Longitude)) >
                MaxDistanceBetweenPositionsInMeters ||
                (message.VehicleIgnition != null && lastMessage.VehicleIgnition != null &&
                 message.VehicleIgnition != lastMessage.VehicleIgnition)
               )
            {
                currentTrip = new InternalTrip();
                trips.Add(currentTrip);
            }

            lastMessage = message;
            currentTrip.Messages.Add(message);
        }

        trips = trips.Where(x => x.Messages.Count > 10).ToList();

        RemoveDuplicatePositions(trips);
        TrimPositionsEnds(trips);
        AddDistance(trips);

        return trips;
    }

    private static void TrimPositionsEnds(List<InternalTrip> trips)
    {
        foreach (InternalTrip trip in trips)
        {
            int firstMovingPosition = trip.Messages.FindIndex(x => x.Speed > 0);
            int lastMovingPosition = trip.Messages.FindLastIndex(x => x.Speed > 0);

            int firstPositionIndex = firstMovingPosition > 1 ? firstMovingPosition - 1 : 0;
            int lastPositionIndex = lastMovingPosition != -1 && lastMovingPosition < trip.Messages.Count - 2
                ? lastMovingPosition + 1
                : trip.Messages.Count - 1;

            trip.Messages = trip.Messages.GetRange(firstPositionIndex, lastPositionIndex - firstPositionIndex + 1);
        }
    }

    private static void RemoveDuplicatePositions(List<InternalTrip> trips)
    {
        const double precision = 0.000001;

        foreach (InternalTrip trip in trips)
        {
            List<DeviceMessageEntity> messages = [];

            DeviceMessageEntity? lastMessage = null;

            foreach (DeviceMessageEntity message in trip.Messages)
            {
                if (lastMessage == null ||
                    Math.Abs(lastMessage.Latitude - message.Latitude) > precision ||
                    Math.Abs(lastMessage.Longitude - message.Longitude) > precision)
                {
                    messages.Add(message);
                    lastMessage = message;
                }
            }

            trip.Messages = messages;
        }
    }

    private static void AddDistance(List<InternalTrip> trips)
    {
        foreach (InternalTrip trip in trips)
        {
            if (trip.StartMessage.DeviceOdometer != null && trip.EndMessage.DeviceOdometer != null)
            {
                trip.Distance = trip.EndMessage.DeviceOdometer.Value - trip.StartMessage.DeviceOdometer.Value;
            }
            else
            {
                trip.Distance = DistanceCalculator.CalculateDistance(trip.Messages
                    .Select(x => (new LatLong(x.Latitude, x.Longitude), (uint?)null)).ToList());
            }
        }
    }

    private async Task<List<DeviceMessageEntity>> GetMessages(Guid assetId, DateFilterModel dateFilter)
    {
        GetMessagesOptions options = new()
        {
            AssetId = assetId,
            PositionFilter = new PositionFilterModel
            {
                StartDate = dateFilter.StartDate?.AddHours(-6),
                EndDate = dateFilter.EndDate?.AddHours(6)
            },
            OrderFunc = query => query.OrderBy(x => x.Date)
        };

        GetMessagesResult result = await repository.GetMessages(options);

        return result.Messages;
    }

    private static List<InternalTrip> ApplyFiltering(List<InternalTrip> trips, TripFilterModel tripFilter)
    {
        List<InternalTrip> filteredTrips = trips.ToList();

        filteredTrips = ApplyDistanceFilter(filteredTrips);
        filteredTrips = ApplyDateFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyAvgAltitudeFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyAvgSpeedFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyDurationFilter(tripFilter, filteredTrips);
        filteredTrips = ApplyGeofenceFilter(tripFilter, filteredTrips);

        return filteredTrips;
    }

    private static List<InternalTrip> ApplyDistanceFilter(List<InternalTrip> filteredTrips)
    {
        filteredTrips = filteredTrips
            .Where(x => x.Distance > MinTripDistanceInMeters)
            .ToList();

        return filteredTrips;
    }

    private static List<InternalTrip> ApplyGeofenceFilter(TripFilterModel tripFilter, List<InternalTrip> filteredTrips)
    {
        if (tripFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            filteredTrips = filteredTrips
                .Where(x => x.Messages
                    .Any(y => DistanceCalculator.IsInRadius(
                        new LatLong(y.Latitude, y.Longitude),
                        new LatLong(tripFilter.Latitude.Value, tripFilter.Longitude.Value),
                        tripFilter.Radius.Value)))
                .ToList();
        }

        return filteredTrips;
    }

    private static List<InternalTrip> ApplyDurationFilter(TripFilterModel tripFilter, List<InternalTrip> filteredTrips)
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

    private static List<InternalTrip> ApplyAvgSpeedFilter(TripFilterModel tripFilter, List<InternalTrip> filteredTrips)
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

    private static List<InternalTrip> ApplyAvgAltitudeFilter(TripFilterModel tripFilter, List<InternalTrip> filteredTrips)
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

    private static List<InternalTrip> ApplyDateFilter(DateFilterModel dateFilter, List<InternalTrip> filteredTrips)
    {
        if (dateFilter.StartDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartMessage.Date >= dateFilter.StartDate.Value.Date).ToList();
        }

        if (dateFilter.EndDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartMessage.Date <= dateFilter.EndDate.Value.Date.AddDays(1)).ToList();
        }

        return filteredTrips;
    }

    private static List<InternalTrip> ApplyOrdering(List<InternalTrip> trips)
    {
        return trips.OrderBy(x => x.StartMessage.Date).ToList();
    }
}