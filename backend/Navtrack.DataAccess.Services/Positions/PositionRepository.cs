using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Model.New;
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

    public Task<List<PositionGroupDocument>> GetLocations(string assetId, LocationFilter locationFilter)
    {
        FilterDefinition<PositionElement> filter = Builders<PositionElement>.Filter.Empty;

        filter = ApplyAltitudeFilter(locationFilter, filter);
        filter = ApplySpeedFilter(locationFilter, filter);
        filter = ApplyGeofenceFilter(locationFilter, filter);
        filter = ApplyValidFilter(filter);
        
        FilterDefinition<PositionGroupDocument> positionGroupFilter = Builders<PositionGroupDocument>.Filter
            .Eq(x => x.AssetId, ObjectId.Parse(assetId));
        
        positionGroupFilter &= ApplyDateFilter(locationFilter, positionGroupFilter);
        positionGroupFilter &= Builders<PositionGroupDocument>.Filter.ElemMatch(x => x.Positions, filter);

        return repository.GetCollection<PositionGroupDocument>()
            .Find(positionGroupFilter)
            .SortBy(x => x.StartDate)
            .ToListAsync();
    }

    public Task<List<PositionGroupDocument>> GetLocations(string assetId, DateFilter dateFilter)
    {
        FilterDefinitionBuilder<PositionGroupDocument> positionGroupFilterBuilder =
            Builders<PositionGroupDocument>.Filter;

        FilterDefinitionBuilder<PositionElement> positionFilterBuilder = Builders<PositionElement>.Filter;
        FilterDefinition<PositionElement> filter = positionFilterBuilder.Empty;

        FilterDefinition<PositionGroupDocument> positionGroupFilter =
            positionGroupFilterBuilder.Eq(x => x.AssetId, ObjectId.Parse(assetId));

        positionGroupFilter = ApplyDateFilter(dateFilter, positionGroupFilter);

        positionGroupFilter &= Builders<PositionGroupDocument>.Filter.ElemMatch(x => x.Positions, filter);

        return repository.GetCollection<PositionGroupDocument>()
            .Find(positionGroupFilter)
            .SortBy(x => x.StartDate)
            .ToListAsync();
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
        return repository.GetCollection<LocationDocument>()
            .DeleteManyAsync(x => x.AssetId == ObjectId.Parse(assetId));
    }

    public Task AddRange(IEnumerable<LocationDocument> locations)
    {
        return repository.GetCollection<LocationDocument>().InsertManyAsync(locations);
    }

    public Task<bool> DeviceHasLocations(string assetId, string deviceId)
    {
        return repository.GetQueryable<PositionGroupDocument>()
            .AnyAsync(x => x.DeviceId == ObjectId.Parse(deviceId) && x.AssetId == ObjectId.Parse(assetId));
    }

    private static FilterDefinition<PositionElement> ApplyValidFilter(FilterDefinition<PositionElement> filter)
    {
        filter &= Builders<PositionElement>.Filter.Exists(x => x.Valid, false) |
                  Builders<PositionElement>.Filter.Eq(x => x.Valid, true);

        return filter;
    }

    private static FilterDefinition<PositionElement> ApplyGeofenceFilter(LocationFilter locationFilter,
        FilterDefinition<PositionElement> filter)
    {
        if (locationFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            GeoJsonPoint<GeoJson2DGeographicCoordinates> center = GeoJson.Point(
                new GeoJson2DGeographicCoordinates(locationFilter.Longitude.Value, locationFilter.Latitude.Value));

            filter &= Builders<PositionElement>.Filter.Near(x => x.Coordinates, center, locationFilter.Radius);
        }

        return filter;
    }

    private static FilterDefinition<PositionElement> ApplySpeedFilter(LocationFilter locationFilter,
        FilterDefinition<PositionElement> filter)
    {
        if (locationFilter.MinSpeed.HasValue)
        {
            filter &= Builders<PositionElement>.Filter.Gte(x => x.Speed, locationFilter.MinSpeed.Value);
        }

        if (locationFilter.MaxSpeed.HasValue)
        {
            filter &= Builders<PositionElement>.Filter.Lte(x => x.Speed, locationFilter.MaxSpeed.Value);
        }

        return filter;
    }

    private static FilterDefinition<PositionGroupDocument> ApplyDateFilter(DateFilter locationFilter,
        FilterDefinition<PositionGroupDocument> filter)
    {
        if (locationFilter.StartDate.HasValue)
        {
            filter &= Builders<PositionGroupDocument>.Filter.Gte(x => x.StartDate, locationFilter.StartDate.Value);
        }

        if (locationFilter.EndDate.HasValue)
        {
            filter &= Builders<PositionGroupDocument>.Filter.Lte(x => x.EndDate, locationFilter.EndDate.Value);
        }

        return filter;
    }

    private static FilterDefinition<PositionElement> ApplyAltitudeFilter(LocationFilter locationFilter,
        FilterDefinition<PositionElement> filter)
    {
        if (locationFilter.MinAltitude.HasValue)
        {
            filter &= Builders<PositionElement>.Filter.Gte(x => x.Altitude, locationFilter.MinAltitude.Value);
        }

        if (locationFilter.MaxAltitude.HasValue)
        {
            filter &= Builders<PositionElement>.Filter.Lte(x => x.Altitude, locationFilter.MaxAltitude.Value);
        }

        return filter;
    }
}