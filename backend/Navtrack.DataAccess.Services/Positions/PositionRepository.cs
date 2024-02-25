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

[Service(typeof(IPositionRepository))]
public class PositionRepository(IRepository repository)
    : GenericRepository<PositionDocument>(repository), IPositionRepository
{
    public async Task<GetPositionsResult> GetPositions(GetPositionsOptions options)
    {
        bool hasPagination = options is { Page: not null, Size: not null };

        FilterDefinition<PositionDocument> filter = GetFilter(options);

        IFindFluent<PositionDocument, PositionDocument>? query = repository.GetCollection<PositionDocument>()
            .Find(filter);

        long count = await query.CountDocumentsAsync();

        query = query.Sort(options.OrderFunc ?? Builders<PositionDocument>.Sort.Descending(x => x.Date));

        IFindFluent<PositionDocument, PositionDocument> paginatedQuery = hasPagination
            ? query.Skip(options.Page!.Value * options.Size!.Value).Limit(options.Size.Value)
            : query;

        List<PositionDocument> positions = await paginatedQuery.ToListAsync();

        GetPositionsResult result = new()
        {
            TotalCount = count,
            Positions = positions
        };

        return result;
    }

    private static FilterDefinition<PositionDocument> GetFilter(GetPositionsOptions options)
    {
        FilterDefinition<PositionDocument> filter = Builders<PositionDocument>.Filter
            .Eq(x => x.Metadata.AssetId, ObjectId.Parse(options.AssetId));

        filter = ApplyDateFilter(options.PositionFilter, filter);
        filter = ApplyAltitudeFilter(options.PositionFilter, filter);
        filter = ApplySpeedFilter(options.PositionFilter, filter);
        filter = ApplyGeofenceFilter(options.PositionFilter, filter);
        filter = ApplyValidFilter(filter);

        return filter;
    }

    private static FilterDefinition<PositionDocument> ApplyDateFilter(DateFilter locationFilter,
        FilterDefinition<PositionDocument> filter)
    {
        if (locationFilter.StartDate.HasValue)
        {
            filter &= Builders<PositionDocument>.Filter.Gte(x => x.Date,
                locationFilter.StartDate.Value);
        }

        if (locationFilter.EndDate.HasValue)
        {
            locationFilter.EndDate = locationFilter.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            filter &= Builders<PositionDocument>.Filter.Lte(x => x.Date,
                locationFilter.EndDate.Value);
        }

        return filter;
    }

    private static FilterDefinition<PositionDocument> ApplyGeofenceFilter(PositionFilter positionFilter,
        FilterDefinition<PositionDocument> filter)
    {
        if (positionFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            double radiusInKm = positionFilter.Radius.Value / 1000d;

            filter &= Builders<PositionDocument>.Filter.GeoWithinCenterSphere(x => x.Coordinates,
                positionFilter.Longitude.Value, positionFilter.Latitude.Value, radiusInKm/6378.1);
        }

        return filter;
    }

    public async Task<Dictionary<ObjectId, int>> GetLocationsCountByDeviceIds(IEnumerable<ObjectId> deviceIds)
    {
        var counts = await repository.GetQueryable<PositionDocument>()
            .Where(x => deviceIds.Contains(x.Metadata.DeviceId))
            .GroupBy(x => x.Metadata.DeviceId)
            .Select(x => new { DeviceId = x.Key, Count = x.Count() })
            .ToListAsync();

        return counts.ToDictionary(x => x.DeviceId, x => x.Count);
    }

    public Task DeleteByAssetId(string assetId)
    {
        return repository.GetCollection<PositionDocument>()
            .DeleteManyAsync(x => x.Metadata.AssetId == ObjectId.Parse(assetId));
    }

    public Task<bool> DeviceHasLocations(string assetId, string deviceId)
    {
        return repository.GetQueryable<PositionDocument>()
            .AnyAsync(x =>
                x.Metadata.DeviceId == ObjectId.Parse(deviceId) && x.Metadata.AssetId == ObjectId.Parse(assetId));
    }

    private static FilterDefinition<PositionDocument> ApplyValidFilter(
        FilterDefinition<PositionDocument> filter)
    {
        filter &= Builders<PositionDocument>.Filter.Exists(x => x.Valid, false) |
                  Builders<PositionDocument>.Filter.Eq(x => x.Valid, true);

        return filter;
    }

    private static FilterDefinition<PositionDocument> ApplySpeedFilter(PositionFilter positionFilter,
        FilterDefinition<PositionDocument> filter)
    {
        if (positionFilter.MinSpeed.HasValue)
        {
            filter &= Builders<PositionDocument>.Filter.Gte(x => x.Speed,
                positionFilter.MinSpeed.Value);
        }

        if (positionFilter.MaxSpeed.HasValue)
        {
            filter &= Builders<PositionDocument>.Filter.Lte(x => x.Speed,
                positionFilter.MaxSpeed.Value);
        }

        return filter;
    }

    private static FilterDefinition<PositionDocument> ApplyAltitudeFilter(PositionFilter positionFilter,
        FilterDefinition<PositionDocument> filter)
    {
        if (positionFilter.MinAltitude.HasValue)
        {
            filter &= Builders<PositionDocument>.Filter.Gte(x => x.Altitude,
                positionFilter.MinAltitude.Value);
        }

        if (positionFilter.MaxAltitude.HasValue)
        {
            filter &= Builders<PositionDocument>.Filter.Lte(x => x.Altitude,
                positionFilter.MaxAltitude.Value);
        }

        return filter;
    }
}