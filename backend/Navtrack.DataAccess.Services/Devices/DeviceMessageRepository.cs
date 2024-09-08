using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Devices;

[Service(typeof(IDeviceMessageRepository))]
public class DeviceMessageRepository(IRepository repository)
    : GenericRepository<DeviceMessageDocument>(repository), IDeviceMessageRepository
{
    public async Task<GetMessagesResult> GetMessages(GetMessagesOptions options)
    {
        bool hasPagination = options is { Page: not null, Size: not null };

        FilterDefinition<DeviceMessageDocument> filter = GetFilter(options);

        IFindFluent<DeviceMessageDocument, DeviceMessageDocument>? query = repository
            .GetCollection<DeviceMessageDocument>()
            .Find(filter);

        long count = await query.CountDocumentsAsync();

        query = query.Sort(options.OrderFunc ?? Builders<DeviceMessageDocument>.Sort.Descending(x => x.Position.Date));

        IFindFluent<DeviceMessageDocument, DeviceMessageDocument> paginatedQuery = hasPagination
            ? query.Skip(options.Page!.Value * options.Size!.Value).Limit(options.Size.Value)
            : query;

        List<DeviceMessageDocument> positions = await paginatedQuery.ToListAsync();

        GetMessagesResult result = new()
        {
            TotalCount = count,
            Messages = positions
        };

        return result;
    }

    private static FilterDefinition<DeviceMessageDocument> GetFilter(GetMessagesOptions options)
    {
        FilterDefinition<DeviceMessageDocument> filter = Builders<DeviceMessageDocument>.Filter
            .Eq(x => x.Metadata.AssetId, ObjectId.Parse(options.AssetId));

        filter = ApplyPositionDateFilter(options.PositionFilter, filter);
        filter = ApplyPositionAltitudeFilter(options.PositionFilter, filter);
        filter = ApplyPositionSpeedFilter(options.PositionFilter, filter);
        filter = ApplyPositionGeofenceFilter(options.PositionFilter, filter);
        filter = ApplyPositionValidFilter(filter);

        return filter;
    }

    private static FilterDefinition<DeviceMessageDocument> ApplyPositionDateFilter(DateFilter locationFilter,
        FilterDefinition<DeviceMessageDocument> filter)
    {
        if (locationFilter.StartDate.HasValue)
        {
            filter &= Builders<DeviceMessageDocument>.Filter.Gte(x => x.Position.Date,
                locationFilter.StartDate.Value);
        }

        if (locationFilter.EndDate.HasValue)
        {
            locationFilter.EndDate = locationFilter.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            filter &= Builders<DeviceMessageDocument>.Filter.Lte(x => x.Position.Date,
                locationFilter.EndDate.Value);
        }

        return filter;
    }

    private static FilterDefinition<DeviceMessageDocument> ApplyPositionGeofenceFilter(PositionFilter positionFilter,
        FilterDefinition<DeviceMessageDocument> filter)
    {
        if (positionFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            double radiusInKm = positionFilter.Radius.Value / 1000d;

            filter &= Builders<DeviceMessageDocument>.Filter.GeoWithinCenterSphere(x => x.Position.Coordinates,
                positionFilter.Longitude.Value, positionFilter.Latitude.Value, radiusInKm / 6378.1);
        }

        return filter;
    }

    public async Task<Dictionary<ObjectId, int>> GetMessagesCountByDeviceIds(IEnumerable<ObjectId> deviceIds)
    {
        var counts = await repository.GetQueryable<DeviceMessageDocument>()
            .Where(x => deviceIds.Contains(x.Metadata.DeviceId))
            .GroupBy(x => x.Metadata.DeviceId)
            .Select(x => new { DeviceId = x.Key, Count = x.Count() })
            .ToListAsync();

        return counts.ToDictionary(x => x.DeviceId, x => x.Count);
    }

    public Task DeleteByAssetId(string assetId)
    {
        return repository.GetCollection<DeviceMessageDocument>()
            .DeleteManyAsync(x => x.Metadata.AssetId == ObjectId.Parse(assetId));
    }

    public Task<bool> DeviceHasMessages(string assetId, string deviceId)
    {
        return repository.GetQueryable<DeviceMessageDocument>()
            .AnyAsync(x =>
                x.Metadata.DeviceId == ObjectId.Parse(deviceId) && x.Metadata.AssetId == ObjectId.Parse(assetId));
    }

    public async Task<(DeviceMessageDocument? first, DeviceMessageDocument? last)> GetFirstAndLast(string assetId,
        DateTime? startDate, DateTime? endDate)
    {
        IMongoQueryable<DeviceMessageDocument> query = repository.GetQueryable<DeviceMessageDocument>()
            .Where(x => x.Metadata.AssetId == ObjectId.Parse(assetId) &&
                        x.Position.Date >= startDate &&
                        x.Position.Date <= endDate);

        DeviceMessageDocument? first = await query.OrderBy(x => x.Position.Date).FirstOrDefaultAsync();
        DeviceMessageDocument? last = await query.OrderByDescending(x => x.Position.Date).FirstOrDefaultAsync();

        return (first, last);
    }

    public async Task<DeviceMessageDocument?> GetFirstOdometer(string assetId)
    {
        DeviceMessageDocument? first = await repository.GetQueryable<DeviceMessageDocument>()
            .Where(x => x.Metadata.AssetId == ObjectId.Parse(assetId) && x.Device != null && x.Device.Odometer != null)
            .OrderBy(x => x.Position.Date)
            .FirstOrDefaultAsync();

        return first;
    }

    private static FilterDefinition<DeviceMessageDocument> ApplyPositionValidFilter(
        FilterDefinition<DeviceMessageDocument> filter)
    {
        filter &= Builders<DeviceMessageDocument>.Filter.Exists(x => x.Position.Valid, false) |
                  Builders<DeviceMessageDocument>.Filter.Eq(x => x.Position.Valid, true);

        return filter;
    }

    private static FilterDefinition<DeviceMessageDocument> ApplyPositionSpeedFilter(PositionFilter positionFilter,
        FilterDefinition<DeviceMessageDocument> filter)
    {
        if (positionFilter.MinSpeed.HasValue)
        {
            filter &= Builders<DeviceMessageDocument>.Filter.Gte(x => x.Position.Speed,
                positionFilter.MinSpeed.Value);
        }

        if (positionFilter.MaxSpeed.HasValue)
        {
            filter &= Builders<DeviceMessageDocument>.Filter.Lte(x => x.Position.Speed,
                positionFilter.MaxSpeed.Value);
        }

        return filter;
    }

    private static FilterDefinition<DeviceMessageDocument> ApplyPositionAltitudeFilter(PositionFilter positionFilter,
        FilterDefinition<DeviceMessageDocument> filter)
    {
        if (positionFilter.MinAltitude.HasValue)
        {
            filter &= Builders<DeviceMessageDocument>.Filter.Gte(x => x.Position.Altitude,
                positionFilter.MinAltitude.Value);
        }

        if (positionFilter.MaxAltitude.HasValue)
        {
            filter &= Builders<DeviceMessageDocument>.Filter.Lte(x => x.Position.Altitude,
                positionFilter.MaxAltitude.Value);
        }

        return filter;
    }
}