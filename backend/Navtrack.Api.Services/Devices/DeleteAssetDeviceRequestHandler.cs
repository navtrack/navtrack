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
    public override async Task Handle(DeleteAssetDeviceRequest request)
    {
        AssetEntity? asset = await assetRepository.GetById(request.AssetId);
        asset.Return404IfNull();

        DeviceEntity? device = await deviceRepository.GetById(request.DeviceId);
        device.Return404IfNull();

        if (await deviceRepository.IsActive(asset.Id, device.Id))
        {
            throw new ApiException(ApiErrorCodes.Device_DeviceIsActive);
        }

        if (await deviceMessageRepository.DeviceHasMessages(asset.Id, device.Id))
        {
            throw new ApiException(ApiErrorCodes.Device_DeviceHasMessages);
        }

        await deviceRepository.Delete(device);
    }
}
