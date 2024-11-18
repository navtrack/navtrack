using System;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Stats;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Stats.Mappers;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Stats;

[Service(typeof(IRequestHandler<GetAssetStatsRequest, AssetStatList>))]
public class GetAssetStatsRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository) : BaseRequestHandler<GetAssetStatsRequest, AssetStatList>
{
    private AssetDocument? asset;

    public override async Task Validate(RequestValidationContext<GetAssetStatsRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<AssetStatList> Handle(GetAssetStatsRequest request)
    {
        DeviceMessageDocument? initialOdometer = await deviceMessageRepository.GetFirstOdometer(request.AssetId);

        AssetStatList result = new()
        {
            Items = await Task.WhenAll(Enum.GetValues<AssetStatsDateRange>().Select(async dateRange =>
            {
                DateTime? middle = AssetStatsDateRangeMapper.GetMidDate(dateRange);
                DateTime? first = AssetStatsDateRangeMapper.GetFirstDate(dateRange, middle);
                DateTime? last = AssetStatsDateRangeMapper.GetEndDate(dateRange, middle);

                GetFirstAndLastPositionResult currentRangePositions =
                    await deviceMessageRepository.GetFirstAndLast(asset!.Id, middle, last);
                GetFirstAndLastPositionResult previousRangePositions =
                    await deviceMessageRepository.GetFirstAndLast(asset!.Id, first, middle);

                AssetStatItem model =
                    AssetStatItemMapper.Map(dateRange, initialOdometer, currentRangePositions, previousRangePositions);

                return model;
            }))
        };

        return result;
    }
}