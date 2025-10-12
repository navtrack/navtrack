using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<GetAssetRequest, AssetModel>))]
public class GetAssetRequestHandler(IAssetRepository assetRepository, IDeviceTypeRepository deviceTypeRepository)
    : BaseRequestHandler<GetAssetRequest, AssetModel>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetAssetRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override Task<AssetModel> Handle(GetAssetRequest request)
    {
        DeviceType? deviceType = asset!.Device != null ? deviceTypeRepository.GetById(asset.Device.DeviceTypeId) : null;

        AssetModel result = AssetMapper.Map(asset, deviceType);

        return Task.FromResult(result);
    }
}