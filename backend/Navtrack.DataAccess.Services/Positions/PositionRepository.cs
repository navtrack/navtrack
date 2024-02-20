using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Positions;

[Service(typeof(IPositionRepository))]
public class PositionRepository(IRepository repository)
    : GenericRepository<PositionGroupDocument>(repository), IPositionRepository
{
    public Task AddPositions(ObjectId positionGroupId, DateTime maxEndDate, IEnumerable<PositionElement> positions)
    {
        return repository.GetCollection<PositionGroupDocument>()
            .UpdateOneAsync(x => x.Id == positionGroupId,
                Builders<PositionGroupDocument>.Update
                    .PushEach(x => x.Positions, positions)
                    .Set(x => x.EndDate, maxEndDate));
    }

    public async Task<GetPositionsResult> GetPositions(GetPositionsOptions options)
    {
        bool hasPagination = options is { Page: not null, Size: not null };

        FilterDefinition<UnwindPositionGroupDocument> filter = Builders<UnwindPositionGroupDocument>.Filter
            .Eq(x => x.AssetId, ObjectId.Parse(options.AssetId));

        filter = ApplyDateFilter(options.PositionFilter, filter);
        filter = ApplyAltitudeFilter(options.PositionFilter, filter);
        filter = ApplySpeedFilter(options.PositionFilter, filter);
        filter = ApplyGeofenceFilter(options.PositionFilter, filter);
        filter = ApplyValidFilter(filter);

        IAggregateFluent<UnwindPositionGroupDocument> aggregateFluent = repository
            .GetCollection<PositionGroupDocument>()
            .Aggregate()
            .Unwind<PositionGroupDocument, UnwindPositionGroupDocument>(x => x.Positions)
            .Match(filter);

        // TODO the queries above could probably be combined into one query with a facet
        AggregateCountResult? count = hasPagination
            ? await aggregateFluent
                .Count()
                .FirstOrDefaultAsync()
            : null;

        IOrderedAggregateFluent<UnwindPositionGroupDocument>? sortedAggregateFluent = options.OrderFunc != null
            ? options.OrderFunc(aggregateFluent)
            : aggregateFluent
                .SortByDescending(x => x.Position.Date);

        List<UnwindPositionGroupDocument>? positions = hasPagination
            ? await sortedAggregateFluent
                .Skip(options.Page!.Value * options.Size!.Value)
                .Limit(options.Size.Value)
                .ToListAsync()
            : await sortedAggregateFluent.ToListAsync();

        GetPositionsResult result = new()
        {
            TotalCount = count?.Count ?? positions.Count,
            Positions = positions
                .Select(x => x.Position)
                .ToList()
        };

        return result;
    }

    private static FilterDefinition<UnwindPositionGroupDocument> ApplyDateFilter(DateFilter locationFilter,
        FilterDefinition<UnwindPositionGroupDocument> filter)
    {
        if (locationFilter.StartDate.HasValue)
        {
            filter &= Builders<UnwindPositionGroupDocument>.Filter.Gte(x => x.Position.Date,
                locationFilter.StartDate.Value);
        }

        if (locationFilter.EndDate.HasValue)
        {
            locationFilter.EndDate = locationFilter.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            filter &= Builders<UnwindPositionGroupDocument>.Filter.Lte(x => x.Position.Date,
                locationFilter.EndDate.Value);
        }

        return filter;
    }

    public async Task<Dictionary<ObjectId, int>> GetLocationsCountByDeviceIds(IEnumerable<ObjectId> deviceIds)
    {
        var counts = await repository.GetQueryable<PositionGroupDocument>()
            .Where(x => deviceIds.Contains(x.DeviceId))
            .GroupBy(x => x.DeviceId)
            .Select(x => new { DeviceId = x.Key, Count = x.Count() })
            .ToListAsync();

        return counts.ToDictionary(x => x.DeviceId, x => x.Count);
    }

    public Task DeleteByAssetId(string assetId)
    {
        return repository.GetCollection<PositionGroupDocument>()
            .DeleteManyAsync(x => x.AssetId == ObjectId.Parse(assetId));
    }

    public Task<bool> DeviceHasLocations(string assetId, string deviceId)
    {
        return repository.GetQueryable<PositionGroupDocument>()
            .AnyAsync(x => x.DeviceId == ObjectId.Parse(deviceId) && x.AssetId == ObjectId.Parse(assetId));
    }

    private static FilterDefinition<UnwindPositionGroupDocument> ApplyValidFilter(
        FilterDefinition<UnwindPositionGroupDocument> filter)
    {
        filter &= Builders<UnwindPositionGroupDocument>.Filter.Exists(x => x.Position.Valid, false) |
                  Builders<UnwindPositionGroupDocument>.Filter.Eq(x => x.Position.Valid, true);

        return filter;
    }

    private static FilterDefinition<UnwindPositionGroupDocument> ApplyGeofenceFilter(PositionFilter positionFilter,
        FilterDefinition<UnwindPositionGroupDocument> filter)
    {
        if (positionFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            GeoJsonPoint<GeoJson2DGeographicCoordinates> center = GeoJson.Point(
                new GeoJson2DGeographicCoordinates(positionFilter.Longitude.Value, positionFilter.Latitude.Value));

            filter &= Builders<UnwindPositionGroupDocument>.Filter.Near(x => x.Position.Coordinates, center,
                positionFilter.Radius);
        }

        return filter;
    }

    private static FilterDefinition<UnwindPositionGroupDocument> ApplySpeedFilter(PositionFilter positionFilter,
        FilterDefinition<UnwindPositionGroupDocument> filter)
    {
        if (positionFilter.MinSpeed.HasValue)
        {
            filter &= Builders<UnwindPositionGroupDocument>.Filter.Gte(x => x.Position.Speed,
                positionFilter.MinSpeed.Value);
        }

        if (positionFilter.MaxSpeed.HasValue)
        {
            filter &= Builders<UnwindPositionGroupDocument>.Filter.Lte(x => x.Position.Speed,
                positionFilter.MaxSpeed.Value);
        }

        return filter;
    }

    private static FilterDefinition<UnwindPositionGroupDocument> ApplyAltitudeFilter(PositionFilter positionFilter,
        FilterDefinition<UnwindPositionGroupDocument> filter)
    {
        if (positionFilter.MinAltitude.HasValue)
        {
            filter &= Builders<UnwindPositionGroupDocument>.Filter.Gte(x => x.Position.Altitude,
                positionFilter.MinAltitude.Value);
        }

        if (positionFilter.MaxAltitude.HasValue)
        {
            filter &= Builders<UnwindPositionGroupDocument>.Filter.Lte(x => x.Position.Altitude,
                positionFilter.MaxAltitude.Value);
        }

        return filter;
    }
}