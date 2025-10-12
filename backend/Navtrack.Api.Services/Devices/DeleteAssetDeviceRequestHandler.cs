using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IRequestHandler<DeleteAssetDeviceRequest>))]
public class DeleteAssetDeviceRequestHandler(
    IAssetRepository assetRepository,
    IDeviceRepository deviceRepository,
    IDeviceMessageRepository deviceMessageRepository) : BaseRequestHandler<DeleteAssetDeviceRequest>
{
    private AssetEntity? asset;
    private DeviceEntity? device;

    public override async Task Validate(RequestValidationContext<DeleteAssetDeviceRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();

        device = await deviceRepository.GetById(context.Request.DeviceId);
        device.Return404IfNull();

        if (await deviceRepository.IsActive(asset.Id, device.Id))
        {
            throw new ApiException(ApiErrorCodes.Device_000003_DeviceIsActive);
        }

        if (await deviceMessageRepository.DeviceHasMessages(asset.Id, device.Id))
        {
            throw new ApiException(ApiErrorCodes.Device_000004_DeviceHasMessages);
        }
    }

    public override Task Handle(DeleteAssetDeviceRequest request)
    {
        return deviceRepository.Delete(device!);
    }
}