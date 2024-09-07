using System;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Stats;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Stats;

[Service(typeof(IAssetStatsService))]
public class AssetStatsService(IDeviceMessageRepository deviceMessageRepository) : IAssetStatsService
{
    public async Task<AssetStatsListModel> GetStats(string assetId)
    {
        AssetStatsListModel model = new()
        {
            Items = await Task.WhenAll(Enum.GetValues<AssetStatsDateRange>().Select(async dateRange =>
            {
                DateTime middle = AssetStatsDateRangeMapper.GetMidDate(dateRange);
                DateTime first = AssetStatsDateRangeMapper.GetFirstDate(dateRange, middle);
                DateTime last = AssetStatsDateRangeMapper.GetEndDate(dateRange, middle);

                (DeviceMessageDocument? first, DeviceMessageDocument? last) currentPositions =
                    await deviceMessageRepository.GetFirstAndLastPosition(assetId, middle, last);
                (DeviceMessageDocument? first, DeviceMessageDocument? last) previousPositions =
                    await deviceMessageRepository.GetFirstAndLastPosition(assetId, first, middle);

                AssetStatsItemModel model =
                    AssetStatsItemModelMapper.Map(dateRange, currentPositions, previousPositions);

                return model;
            }))
        };

        return model;
    }
}