using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Filters;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Devices;

[Service(typeof(IDeviceMessageRepository))]
public class DeviceMessageRepository(IPostgresRepository repository)
    : GenericPostgresRepository<DeviceMessageEntity>(repository), IDeviceMessageRepository
{
    public async Task<GetMessagesResult> GetMessages(GetMessagesOptions options)
    {
        bool hasPagination = options is { Page: not null, Size: not null };

        IQueryable<DeviceMessageEntity> query = GetFilter(options);

        long count = await query.CountAsync();

        query = query.OrderBy(x => x.Date);

        query = hasPagination
            ? query
                .Skip(options.Page!.Value * options.Size!.Value)
                .Take(options.Size.Value)
            : query;

        List<DeviceMessageEntity> positions = await query.ToListAsync();

        GetMessagesResult result = new()
        {
            TotalCount = count,
            Messages = positions
        };

        return result;
    }

    private IQueryable<DeviceMessageEntity> GetFilter(GetMessagesOptions options)
    {
        IQueryable<DeviceMessageEntity> filter = repository.GetQueryable<DeviceMessageEntity>()
            .Where(x => x.AssetId == options.AssetId);

        filter = ApplyPositionDateFilter(options.PositionFilter, filter);
        filter = ApplyPositionAltitudeFilter(options.PositionFilter, filter);
        filter = ApplyPositionSpeedFilter(options.PositionFilter, filter);
        filter = ApplyPositionGeofenceFilter(options.PositionFilter, filter);
        filter = ApplyPositionValidFilter(filter);

        return filter;
    }

    private static IQueryable<DeviceMessageEntity> ApplyPositionDateFilter(DateFilterModel locationFilter,
        IQueryable<DeviceMessageEntity> filter)
    {
        if (locationFilter.StartDate.HasValue)
        {
            filter = filter.Where(x =>
                x.Date >= DateTime.SpecifyKind(locationFilter.StartDate.Value, DateTimeKind.Utc));
        }

        if (locationFilter.EndDate.HasValue)
        {
            locationFilter.EndDate = locationFilter.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            filter = filter.Where(x => x.Date <= DateTime.SpecifyKind(locationFilter.EndDate.Value, DateTimeKind.Utc));
        }

        return filter;
    }

    private static IQueryable<DeviceMessageEntity> ApplyPositionGeofenceFilter(PositionFilterModel positionFilter,
        IQueryable<DeviceMessageEntity> filter)
    {
        if (positionFilter is { Latitude: not null, Longitude: not null, Radius: not null })
        {
            double radiusInKm = positionFilter.Radius.Value / 1000d;

            // TODO
            // filter &= Builders<DeviceMessageDocument>.Filter.GeoWithinCenterSphere(x => x.Position.Coordinates,
            //     positionFilter.Longitude.Value, positionFilter.Latitude.Value, radiusInKm / 6378.1);
        }

        return filter;
    }

    public async Task<Dictionary<Guid, int>> GetMessagesCountByDeviceIds(IEnumerable<Guid> deviceIds)
    {
        var counts = await repository.GetQueryable<DeviceMessageEntity>()
            .Where(x => deviceIds.Contains(x.DeviceId))
            .GroupBy(x => x.DeviceId)
            .Select(x => new { DeviceId = x.Key, Count = x.Count() })
            .ToListAsync();

        return counts.ToDictionary(x => x.DeviceId, x => x.Count);
    }

    public Task DeleteByAssetId(Guid assetId)
    {
        return repository.GetQueryable<DeviceMessageEntity>()
            .Where(x => x.AssetId == assetId)
            .ExecuteDeleteAsync();
    }

    public Task<bool> DeviceHasMessages(Guid assetId, Guid deviceId)
    {
        return repository.GetQueryable<DeviceMessageEntity>()
            .AnyAsync(x => x.AssetId == assetId && x.DeviceId == deviceId);
    }

    public async Task<GetFirstAndLastPositionResult> GetFirstAndLast(Guid assetId,
        DateTime? startDate, DateTime? endDate)
    {
        IQueryable<DeviceMessageEntity> query = repository.GetQueryable<DeviceMessageEntity>()
            .Where(x => x.AssetId == assetId &&
                        x.Date >= DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc) &&
                        x.Date <= DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc));

        GetFirstAndLastPositionResult result = new()
        {
            FirstOdometer = await query.Where(x => x.DeviceOdometer > 0)
                .OrderBy(x => x.Date)
                .FirstOrDefaultAsync(),
            LastOdometer = await query.Where(x => x.DeviceOdometer > 0)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(),
            FirstFuelConsumption = await query.Where(x => x.VehicleFuelConsumption > 0)
                .OrderBy(x => x.Date)
                .FirstOrDefaultAsync(),
            LastFuelConsumption = await query.Where(x => x.VehicleFuelConsumption > 0)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(),
        };

        return result;
    }

    public async Task<GetFirstAndLastPositionResult> GetFirstAndLast(Guid assetId, DateTime date)
    {
        DateTime startDate = new(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
        DateTime endDate = new(date.Year, date.Month, date.Day, 23, 59, 59, DateTimeKind.Utc);

        IQueryable<DeviceMessageEntity> query = repository.GetQueryable<DeviceMessageEntity>()
            .Where(x => x.AssetId == assetId &&
                        x.Date >= startDate &&
                        x.Date <= endDate);

        GetFirstAndLastPositionResult result = new()
        {
            FirstOdometer = await query.Where(x => x.DeviceOdometer > 0)
                .OrderBy(x => x.Date)
                .FirstOrDefaultAsync(),
            LastOdometer = await query.Where(x => x.DeviceOdometer > 0)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(),
            FirstFuelConsumption = await query.Where(x => x.VehicleFuelConsumption > 0)
                .OrderBy(x => x.Date)
                .FirstOrDefaultAsync(),
            LastFuelConsumption = await query.Where(x => x.VehicleFuelConsumption > 0)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(),
        };

        return result;
    }

    public async Task<DeviceMessageEntity?> GetFirstOdometer(Guid assetId)
    {
        DeviceMessageEntity? first = await repository.GetQueryable<DeviceMessageEntity>()
            .Where(x => x.AssetId == assetId && x.DeviceOdometer > 0)
            .OrderBy(x => x.Date)
            .FirstOrDefaultAsync();

        return first;
    }

    private static IQueryable<DeviceMessageEntity> ApplyPositionValidFilter(
        IQueryable<DeviceMessageEntity> filter)
    {
        filter = filter.Where(x => x.Valid == null || x.Valid == true);

        return filter;
    }

    private static IQueryable<DeviceMessageEntity> ApplyPositionSpeedFilter(PositionFilterModel positionFilter,
        IQueryable<DeviceMessageEntity> filter)
    {
        if (positionFilter.MinSpeed.HasValue)
        {
            filter = filter.Where(x => x.Speed >= positionFilter.MinSpeed.Value);
        }

        if (positionFilter.MaxSpeed.HasValue)
        {
            filter = filter.Where(x => x.Speed <= positionFilter.MaxSpeed.Value);
        }

        return filter;
    }

    private static IQueryable<DeviceMessageEntity> ApplyPositionAltitudeFilter(PositionFilterModel positionFilter,
        IQueryable<DeviceMessageEntity> filter)
    {
        if (positionFilter.MinAltitude.HasValue)
        {
            filter = filter.Where(x => x.Altitude >= positionFilter.MinAltitude.Value);
        }

        if (positionFilter.MaxAltitude.HasValue)
        {
            filter = filter.Where(x => x.Altitude <= positionFilter.MaxAltitude.Value);
        }

        return filter;
    }
}