using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Trips.Mappers;
using Navtrack.Api.Services.Trips.Models;
using Navtrack.DataAccess.Model.Assets;
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

    public async Task<TripList> GetTrips(AssetDocument asset, TripFilter tripFilter)
    {
        MapFilter(asset, tripFilter);
        
        List<InternalTrip> trips = await GetInternalTrips(asset.Id.ToString(), tripFilter);

        TripList list = TripListMapper.Map(trips);

        var filtered = list.Items.Where(x => x.FuelConsumption > 200).ToList();

        return list;
    }

    private static void MapFilter(AssetDocument asset, TripFilter tripFilter)
    {
        if (tripFilter.StartDate == null || tripFilter.EndDate == null)
        {
            DateTime latestPositionDate = asset.LastPositionMessage?.Position.Date ?? DateTime.UtcNow;
            
            tripFilter.StartDate = latestPositionDate.Date;
            tripFilter.EndDate = latestPositionDate.Date;
        }

        if (tripFilter.StartDate == tripFilter.EndDate)
        {
            tripFilter.EndDate = tripFilter.EndDate.Value.AddDays(1);
        }
    }

    private async Task<List<InternalTrip>> GetInternalTrips(string assetId, TripFilter tripFilter)
    {
        List<DeviceMessageDocument> messages = await GetMessages(assetId, tripFilter);

        List<InternalTrip> trips = CreateTrips(messages);

        trips = ApplyFiltering(trips, tripFilter);
        trips = ApplyOrdering(trips);

        return trips;
    }

    private static List<InternalTrip> CreateTrips(List<DeviceMessageDocument> source)
    {
        List<InternalTrip> trips = [];

        InternalTrip? currentTrip = null;
        DeviceMessageDocument? lastMessage = null;

        foreach (DeviceMessageDocument message in source)
        {
            if (currentTrip == null ||
                lastMessage == null ||
                message.Position.Date > lastMessage.Position.Date.AddMinutes(MaxTimeBetweenTripInMinutes) ||
                DistanceCalculator.CalculateDistance(
                    new LatLong(lastMessage.Position.Latitude, lastMessage.Position.Longitude),
                    new LatLong(message.Position.Latitude, message.Position.Longitude)) >
                MaxDistanceBetweenPositionsInMeters ||
                (message.Vehicle?.Ignition != null && lastMessage.Vehicle?.Ignition != null &&
                 message.Vehicle.Ignition != lastMessage.Vehicle.Ignition)
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
            int firstMovingPosition = trip.Messages.FindIndex(x => x.Position.Speed > 0);
            int lastMovingPosition = trip.Messages.FindLastIndex(x => x.Position.Speed > 0);

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
            List<DeviceMessageDocument> messages = [];

            DeviceMessageDocument? lastMessage = null;

            foreach (DeviceMessageDocument message in trip.Messages)
            {
                if (lastMessage == null ||
                    Math.Abs(lastMessage.Position.Latitude - message.Position.Latitude) > precision ||
                    Math.Abs(lastMessage.Position.Longitude - message.Position.Longitude) > precision)
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
            if (trip.StartMessage.Device?.Odometer != null && trip.EndMessage.Device?.Odometer != null)
            {
                trip.Distance = trip.EndMessage.Device.Odometer.Value - trip.StartMessage.Device.Odometer.Value;
            }
            else
            {
                trip.Distance = DistanceCalculator.CalculateDistance(trip.Messages
                    .Select(x => (new LatLong(x.Position.Latitude, x.Position.Longitude), (uint?)null)).ToList());
            }
        }
    }

    private async Task<List<DeviceMessageDocument>> GetMessages(string assetId, DateFilter dateFilter)
    {
        GetMessagesOptions options = new()
        {
            AssetId = assetId,
            PositionFilter = new PositionFilter
            {
                StartDate = dateFilter.StartDate?.AddHours(-6) ?? dateFilter.StartDate,
                EndDate = dateFilter.EndDate?.AddHours(6) ?? dateFilter.EndDate
            },
            OrderFunc = Builders<DeviceMessageDocument>.Sort.Ascending(x => x.Position.Date)
        };

        GetMessagesResult result = await repository.GetMessages(options);

        return result.Messages;
    }

    private static List<InternalTrip> ApplyFiltering(List<InternalTrip> trips, TripFilter tripFilter)
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

    private static List<InternalTrip> ApplyGeofenceFilter(TripFilter tripFilter, List<InternalTrip> filteredTrips)
    {
        if (tripFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            filteredTrips = filteredTrips
                .Where(x => x.Messages
                    .Any(y => DistanceCalculator.IsInRadius(
                        new LatLong(y.Position.Latitude, y.Position.Longitude),
                        new LatLong(tripFilter.Latitude.Value, tripFilter.Longitude.Value),
                        tripFilter.Radius.Value)))
                .ToList();
        }

        return filteredTrips;
    }

    private static List<InternalTrip> ApplyDurationFilter(TripFilter tripFilter, List<InternalTrip> filteredTrips)
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

    private static List<InternalTrip> ApplyAvgSpeedFilter(TripFilter tripFilter, List<InternalTrip> filteredTrips)
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

    private static List<InternalTrip> ApplyAvgAltitudeFilter(TripFilter tripFilter, List<InternalTrip> filteredTrips)
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

    private static List<InternalTrip> ApplyDateFilter(DateFilter dateFilter, List<InternalTrip> filteredTrips)
    {
        if (dateFilter.StartDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartMessage.Position.Date >= dateFilter.StartDate.Value.Date).ToList();
        }

        if (dateFilter.EndDate.HasValue)
        {
            filteredTrips = filteredTrips
                .Where(x => x.StartMessage.Position.Date <= dateFilter.EndDate.Value.Date).ToList();
        }

        return filteredTrips;
    }

    private static List<InternalTrip> ApplyOrdering(List<InternalTrip> trips)
    {
        return trips.OrderBy(x => x.StartMessage.Position.Date).ToList();
    }
}