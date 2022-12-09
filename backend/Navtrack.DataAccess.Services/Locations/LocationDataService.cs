using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Locations;

[Service(typeof(ILocationDataService))]
public class LocationDataService : ILocationDataService
{
    private readonly IRepository repository;

    public LocationDataService(IRepository repository)
    {
        this.repository = repository;
    }

    public Task<List<LocationDocument>> GetLocations(string assetId, LocationFilter locationFilter)
    {
        FilterDefinitionBuilder<LocationDocument> builder = Builders<LocationDocument>.Filter;
        FilterDefinition<LocationDocument> filter = builder.Eq(x => x.AssetId, ObjectId.Parse(assetId));

        filter = ApplyAltitudeFilter(locationFilter, filter, builder);
        filter = ApplyDateFilter(locationFilter, filter, builder);
        filter = ApplySpeedFilter(locationFilter, filter, builder);
        filter = ApplyGeofenceFilter(locationFilter, filter, builder);
        filter = ApplyValidFilter(filter, builder);

        return repository.GetCollection<LocationDocument>()
            .Find(filter)
            .SortByDescending(x => x.DateTime)
            .ToListAsync();
    }

    public Task<List<LocationDocument>> GetLocations(string assetId, DateFilter locationFilter)
    {
        FilterDefinitionBuilder<LocationDocument> builder = Builders<LocationDocument>.Filter;
        FilterDefinition<LocationDocument> filter = builder.Eq(x => x.AssetId, ObjectId.Parse(assetId));

        filter = ApplyDateFilter(locationFilter, filter, builder);

        return repository.GetCollection<LocationDocument>()
            .Find(filter)
            .SortBy(x => x.DateTime)
            .ToListAsync();
    }

    public async Task<Dictionary<ObjectId, int>> GetLocationsCountByDeviceIds(IEnumerable<ObjectId> deviceIds)
    {
        var counts = await repository.GetEntities<LocationDocument>()
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

    private static FilterDefinition<LocationDocument> ApplyValidFilter(FilterDefinition<LocationDocument> filter,
        FilterDefinitionBuilder<LocationDocument> builder)
    {
        filter &= builder.Exists(x => x.Valid, false) |
                  builder.Eq(x => x.Valid, true);

        return filter;
    }

    private static FilterDefinition<LocationDocument> ApplyGeofenceFilter(LocationFilter locationFilter,
        FilterDefinition<LocationDocument> filter,
        FilterDefinitionBuilder<LocationDocument> builder)
    {
        if (locationFilter.Latitude.HasValue && locationFilter.Longitude.HasValue && locationFilter.Radius.HasValue)
        {
            GeoJsonPoint<GeoJson2DGeographicCoordinates> center = GeoJson.Point(
                new GeoJson2DGeographicCoordinates(locationFilter.Longitude.Value, locationFilter.Latitude.Value));

            filter &= builder.Near(x => x.Coordinates, center, locationFilter.Radius);
        }

        return filter;
    }

    private static FilterDefinition<LocationDocument> ApplySpeedFilter(LocationFilter locationFilter,
        FilterDefinition<LocationDocument> filter,
        FilterDefinitionBuilder<LocationDocument> builder)
    {
        if (locationFilter.MinSpeed.HasValue)
        {
            filter &= builder.Gte(x => x.Speed, locationFilter.MinSpeed.Value);
        }

        if (locationFilter.MaxSpeed.HasValue)
        {
            filter &= builder.Lte(x => x.Speed, locationFilter.MaxSpeed.Value);
        }

        return filter;
    }

    private static FilterDefinition<LocationDocument> ApplyDateFilter(DateFilter locationFilter,
        FilterDefinition<LocationDocument> filter,
        FilterDefinitionBuilder<LocationDocument> builder)
    {
        if (locationFilter.StartDate.HasValue)
        {
            filter &= builder.Gte(x => x.DateTime, locationFilter.StartDate.Value);
        }

        if (locationFilter.EndDate.HasValue)
        {
            filter &= builder.Lte(x => x.DateTime, locationFilter.EndDate.Value);
        }

        return filter;
    }

    private static FilterDefinition<LocationDocument> ApplyAltitudeFilter(LocationFilter locationFilter,
        FilterDefinition<LocationDocument> filter,
        FilterDefinitionBuilder<LocationDocument> builder)
    {
        if (locationFilter.MinAltitude.HasValue)
        {
            filter &= builder.Gte(x => x.Altitude, locationFilter.MinAltitude.Value);
        }

        if (locationFilter.MaxAltitude.HasValue)
        {
            filter &= builder.Lte(x => x.Altitude, locationFilter.MaxAltitude.Value);
        }

        return filter;
    }
}