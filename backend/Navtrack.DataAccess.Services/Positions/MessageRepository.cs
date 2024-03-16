using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Positions;

[Service(typeof(IMessageRepository))]
public class MessageRepository(IRepository repository)
    : GenericRepository<MessageDocument>(repository), IMessageRepository
{
    public async Task<GetMessagesResult> GetMessages(GetMessagesOptions options)
    {
        bool hasPagination = options is { Page: not null, Size: not null };

        FilterDefinition<MessageDocument> filter = GetFilter(options);

        IFindFluent<MessageDocument, MessageDocument>? query = repository.GetCollection<MessageDocument>()
            .Find(filter);

        long count = await query.CountDocumentsAsync();

        query = query.Sort(options.OrderFunc ?? Builders<MessageDocument>.Sort.Descending(x => x.Position.Date));

        IFindFluent<MessageDocument, MessageDocument> paginatedQuery = hasPagination
            ? query.Skip(options.Page!.Value * options.Size!.Value).Limit(options.Size.Value)
            : query;

        List<MessageDocument> positions = await paginatedQuery.ToListAsync();

        GetMessagesResult result = new()
        {
            TotalCount = count,
            Messages = positions
        };

        return result;
    }

    private static FilterDefinition<MessageDocument> GetFilter(GetMessagesOptions options)
    {
        FilterDefinition<MessageDocument> filter = Builders<MessageDocument>.Filter
            .Eq(x => x.Metadata.AssetId, ObjectId.Parse(options.AssetId));

        filter = ApplyPositionDateFilter(options.PositionFilter, filter);
        filter = ApplyPositionAltitudeFilter(options.PositionFilter, filter);
        filter = ApplyPositionSpeedFilter(options.PositionFilter, filter);
        filter = ApplyPositionGeofenceFilter(options.PositionFilter, filter);
        filter = ApplyPositionValidFilter(filter);

        return filter;
    }

    private static FilterDefinition<MessageDocument> ApplyPositionDateFilter(DateFilter locationFilter,
        FilterDefinition<MessageDocument> filter)
    {
        if (locationFilter.StartDate.HasValue)
        {
            filter &= Builders<MessageDocument>.Filter.Gte(x => x.Position.Date,
                locationFilter.StartDate.Value);
        }

        if (locationFilter.EndDate.HasValue)
        {
            locationFilter.EndDate = locationFilter.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            filter &= Builders<MessageDocument>.Filter.Lte(x => x.Position.Date,
                locationFilter.EndDate.Value);
        }

        return filter;
    }

    private static FilterDefinition<MessageDocument> ApplyPositionGeofenceFilter(PositionFilter positionFilter,
        FilterDefinition<MessageDocument> filter)
    {
        if (positionFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            double radiusInKm = positionFilter.Radius.Value / 1000d;

            filter &= Builders<MessageDocument>.Filter.GeoWithinCenterSphere(x => x.Position.Coordinates,
                positionFilter.Longitude.Value, positionFilter.Latitude.Value, radiusInKm/6378.1);
        }

        return filter;
    }

    public async Task<Dictionary<ObjectId, int>> GetMessagesCountByDeviceIds(IEnumerable<ObjectId> deviceIds)
    {
        var counts = await repository.GetQueryable<MessageDocument>()
            .Where(x => deviceIds.Contains(x.Metadata.DeviceId))
            .GroupBy(x => x.Metadata.DeviceId)
            .Select(x => new { DeviceId = x.Key, Count = x.Count() })
            .ToListAsync();

        return counts.ToDictionary(x => x.DeviceId, x => x.Count);
    }

    public Task DeleteByAssetId(string assetId)
    {
        return repository.GetCollection<MessageDocument>()
            .DeleteManyAsync(x => x.Metadata.AssetId == ObjectId.Parse(assetId));
    }

    public Task<bool> DeviceHasMessages(string assetId, string deviceId)
    {
        return repository.GetQueryable<MessageDocument>()
            .AnyAsync(x =>
                x.Metadata.DeviceId == ObjectId.Parse(deviceId) && x.Metadata.AssetId == ObjectId.Parse(assetId));
    }

    private static FilterDefinition<MessageDocument> ApplyPositionValidFilter(
        FilterDefinition<MessageDocument> filter)
    {
        filter &= Builders<MessageDocument>.Filter.Exists(x => x.Position.Valid, false) |
                  Builders<MessageDocument>.Filter.Eq(x => x.Position.Valid, true);

        return filter;
    }

    private static FilterDefinition<MessageDocument> ApplyPositionSpeedFilter(PositionFilter positionFilter,
        FilterDefinition<MessageDocument> filter)
    {
        if (positionFilter.MinSpeed.HasValue)
        {
            filter &= Builders<MessageDocument>.Filter.Gte(x => x.Position.Speed,
                positionFilter.MinSpeed.Value);
        }

        if (positionFilter.MaxSpeed.HasValue)
        {
            filter &= Builders<MessageDocument>.Filter.Lte(x => x.Position.Speed,
                positionFilter.MaxSpeed.Value);
        }

        return filter;
    }

    private static FilterDefinition<MessageDocument> ApplyPositionAltitudeFilter(PositionFilter positionFilter,
        FilterDefinition<MessageDocument> filter)
    {
        if (positionFilter.MinAltitude.HasValue)
        {
            filter &= Builders<MessageDocument>.Filter.Gte(x => x.Position.Altitude,
                positionFilter.MinAltitude.Value);
        }

        if (positionFilter.MaxAltitude.HasValue)
        {
            filter &= Builders<MessageDocument>.Filter.Lte(x => x.Position.Altitude,
                positionFilter.MaxAltitude.Value);
        }

        return filter;
    }
}