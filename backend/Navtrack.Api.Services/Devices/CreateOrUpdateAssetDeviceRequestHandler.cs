using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;
using DeviceType = Navtrack.Database.Model.Devices.DeviceType;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IRequestHandler<CreateOrUpdateAssetDeviceRequest>))]
public class CreateOrUpdateAssetDeviceRequestHandler(
    IAssetRepository assetRepository,
    IDeviceRepository deviceRepository,
    IDeviceTypeRepository deviceTypeRepository,
    INavtrackRequestContextAccessor navtrackRequestContextAccessor) : BaseRequestHandler<CreateOrUpdateAssetDeviceRequest>
{
    public override async Task Handle(CreateOrUpdateAssetDeviceRequest request)
    {
        AssetEntity? asset = await assetRepository.GetById(request.AssetId);
        asset.Return404IfNull();

        DeviceType? deviceType = deviceTypeRepository.GetById(request.Model.DeviceTypeId);
        ValidationApiException validationException = new();

        validationException.AddErrorIfNull(
            deviceType, nameof(request.Model.DeviceTypeId), ApiErrorCodes.Device_DeviceTypeInvalid);
        validationException.ThrowIfInvalid();

        validationException.AddErrorIfTrue(
            await deviceRepository.SerialNumberIsUsed(request.Model.SerialNumber, deviceType.Protocol.Port,
                asset.Id),
            nameof(request.Model.SerialNumber), ApiErrorCodes.Device_SerialNumberUsed);
        validationException.ThrowIfInvalid();

        List<DeviceEntity> devices = await deviceRepository.GetDevicesByAssetId(asset.Id);
        CreateOrUpdateAssetDeviceModel model = request.Model;

        DeviceEntity? existingDevice = devices.FirstOrDefault(x =>
            x.DeviceTypeId == model.DeviceTypeId && x.SerialNumber == model.SerialNumber);

        if (existingDevice != null)
        {
            await assetRepository.SetActiveDevice(asset.Id, existingDevice.Id);
        }
        else
        {
            DeviceEntity newDevice = DeviceDocumentMapper.Map(asset, navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id,
                model.SerialNumber, deviceType);

            await deviceRepository.Add(newDevice);

            await assetRepository.SetActiveDevice(asset.Id, newDevice.Id);
        }
    }
}
