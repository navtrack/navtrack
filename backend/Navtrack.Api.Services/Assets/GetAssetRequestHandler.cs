using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<GetAssetRequest, Asset>))]
public class GetAssetRequestHandler(IAssetRepository assetRepository, IDeviceTypeRepository deviceTypeRepository)
    : BaseRequestHandler<GetAssetRequest, Asset>
{
    private AssetDocument? asset;

    public override async Task Validate(RequestValidationContext<GetAssetRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override Task<Asset> Handle(GetAssetRequest request)
    {
        DeviceType? deviceType = asset!.Device != null ? deviceTypeRepository.GetById(asset.Device.DeviceTypeId) : null;

        Asset result = AssetMapper.Map(asset, deviceType);

        return Task.FromResult(result);
    }
}