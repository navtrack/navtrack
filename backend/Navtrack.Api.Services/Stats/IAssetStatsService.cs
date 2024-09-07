using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Stats;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Stats;

public interface IAssetStatsService
{
    Task<AssetStatsListModel> GetStats(string assetId);
}

[Service(typeof(IAssetStatsService))]
public class AssetStatsService : IAssetStatsService
{
    private readonly IDeviceMessageRepository _deviceMessageRepository;

    public AssetStatsService(IDeviceMessageRepository deviceMessageRepository)
    {
        _deviceMessageRepository = deviceMessageRepository;
    }

    public async Task<AssetStatsListModel> GetStats(string assetId)
    {
        AssetStatsListModel model = new();

        IEnumerable<Task<AssetStatsItemModel>> items = Enum.GetValues<AssetStatsDateRange>().Select(async dateRange =>
        {
            DateTime startDate = GetStartDate(dateRange);
            DateTime endDate = GetEndDate(dateRange, startDate);

            AssetStatsItemModel item = new()
            {
                DateRange = dateRange,
                FuelConsumption = new Random().Next(0, 1000),
                PreviousFuelConsumption = new Random().Next(0, 1000),
                Duration = new Random().Next(0, 1000),
                PreviousDuration = new Random().Next(0, 1000)
            };

            await MapStats(assetId, startDate, endDate, item);

            DateTime previousStartDate = GetPreviousDate(dateRange, startDate);
            DateTime previousEndDate = GetPreviousDate(dateRange, endDate);

            await MapPreviousStats(assetId, previousStartDate, previousEndDate, item);

            if (item is { Distance: not null, PreviousDistance: not null } && item.PreviousDistance != 0)
            {
                item.DistanceChange = (int?)((item.Distance.Value - item.PreviousDistance.Value) /
                    (double)item.PreviousDistance.Value * 100);
            }
            else if (item is { Distance: not null, PreviousDistance: null })
            {
                item.DistanceChange = 100;
            }
            else
            {
                item.DistanceChange = -100;
            }

            item.FuelConsumptionChange = (int?)((item.FuelConsumption.Value - item.PreviousFuelConsumption.Value) /
                (double)item.PreviousFuelConsumption.Value * 100);
            item.DurationChange = (int?)((item.Duration.Value - item.PreviousDuration.Value) /
                (double)item.PreviousDuration.Value * 100);

            return item;
        });

        model.Items = await Task.WhenAll(items);

        return model;
    }

    private async Task MapStats(string assetId, DateTime startDate, DateTime endDate, AssetStatsItemModel model)
    {
        (DeviceMessageDocument? first, DeviceMessageDocument? last) =
            await _deviceMessageRepository.GetFirstAndLastPosition(assetId, startDate, endDate);

        if (first?.Device?.Odometer != null && last?.Device?.Odometer != null)
        {
            model.Distance = last.Device.Odometer.Value - first.Device.Odometer.Value;
            // model.Duration = 0;
            // model.FuelConsumption = 0;
        }
    }

    private async Task MapPreviousStats(string assetId, DateTime startDate, DateTime endDate, AssetStatsItemModel model)
    {
        (DeviceMessageDocument? first, DeviceMessageDocument? last) =
            await _deviceMessageRepository.GetFirstAndLastPosition(assetId, startDate, endDate);

        if (first?.Device?.Odometer != null && last?.Device?.Odometer != null)
        {
            model.PreviousDistance = last.Device.Odometer.Value - first.Device.Odometer.Value;

            // model.Duration = 0;
            // model.FuelConsumption = 0;
        }
    }

    private static DateTime GetStartDate(AssetStatsDateRange dateRange)
    {
        DateTime now = DateTime.UtcNow;

        return dateRange switch
        {
            AssetStatsDateRange.ThisWeek => new DateTime(now.Year, now.Month, now.Day)
                .AddDays(-((int)now.DayOfWeek - 1)),
            AssetStatsDateRange.ThisMonth => new DateTime(now.Year, now.Month, 1),
            AssetStatsDateRange.ThisYear => new DateTime(now.Year, 1, 1),
            _ => new DateTime(now.Year, now.Month, now.Day),
        };
    }

    private static DateTime GetEndDate(AssetStatsDateRange dateRange, DateTime startDate)
    {
        return dateRange switch
        {
            AssetStatsDateRange.ThisWeek => startDate.AddDays(7),
            AssetStatsDateRange.ThisMonth => startDate.AddMonths(1),
            AssetStatsDateRange.ThisYear => startDate.AddYears(1),
            _ => startDate.AddDays(1),
        };
    }

    private static DateTime GetPreviousDate(AssetStatsDateRange dateRangeType, DateTime date)
    {
        return dateRangeType switch
        {
            AssetStatsDateRange.ThisWeek => date.AddDays(-7),
            AssetStatsDateRange.ThisMonth => date.AddMonths(-1),
            AssetStatsDateRange.ThisYear => date.AddYears(-1),
            _ => date.AddDays(-1)
        };
    }
}