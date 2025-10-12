using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Stats;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Stats.Mappers;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Stats;

[Service(typeof(IRequestHandler<GetAssetStatsRequest, AssetStatListModel>))]
public class GetAssetStatsRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository) : BaseRequestHandler<GetAssetStatsRequest, AssetStatListModel>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetAssetStatsRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<AssetStatListModel> Handle(GetAssetStatsRequest request)
    {
        DeviceMessageEntity? initialOdometer = await deviceMessageRepository.GetFirstOdometer(asset.Id);

        List<AssetStatItemModel> items = [];
        
        AssetStatsDateRange[] dateRanges = Enum.GetValues<AssetStatsDateRange>();

        foreach (AssetStatsDateRange dateRange in dateRanges)
        {
            DateTime? middle = AssetStatsDateRangeMapper.GetMidDate(dateRange);
            DateTime? first = AssetStatsDateRangeMapper.GetFirstDate(dateRange, middle);
            DateTime? last = AssetStatsDateRangeMapper.GetEndDate(dateRange, middle);

            GetFirstAndLastPositionResult currentRangePositions =
                await deviceMessageRepository.GetFirstAndLast(asset!.Id, middle, last);
            GetFirstAndLastPositionResult previousRangePositions =
                await deviceMessageRepository.GetFirstAndLast(asset!.Id, first, middle);

            AssetStatItemModel model =
                AssetStatItemMapper.Map(dateRange, initialOdometer, currentRangePositions, previousRangePositions);

            items.Add(model);
        }
        
        AssetStatListModel result = new()
        {
            Items = items
        };
        
        return result;
    }
}