using System;
using System.Threading.Tasks;
using Navtrack.Api.Model.Stats;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Stats.Mappers;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Stats;

[Service(typeof(IRequestHandler<GetAssetStatsRequest, AssetStatsModel>))]
public class GetAssetStatsRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository) : BaseRequestHandler<GetAssetStatsRequest, AssetStatsModel>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetAssetStatsRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<AssetStatsModel> Handle(GetAssetStatsRequest request)
    {
        int? initialOdometer = await deviceMessageRepository.GetFirstOdometer(asset!.Id);

        DateTime? middle = AssetStatsDateRangeMapper.GetMidDate(request.Period);
        DateTime? first = AssetStatsDateRangeMapper.GetFirstDate(request.Period, middle);
        DateTime? last = AssetStatsDateRangeMapper.GetEndDate(request.Period, middle);

        GetFirstAndLastPositionResult currentRangePositions =
            await deviceMessageRepository.GetFirstAndLast(asset!.Id, middle, last);
        GetFirstAndLastPositionResult previousRangePositions =
            await deviceMessageRepository.GetFirstAndLast(asset!.Id, first, middle);

        AssetStatsModel model =
            AssetStatItemMapper.Map(initialOdometer, currentRangePositions, previousRangePositions);

        return model;
    }
}