using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
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
    INavtrackContextAccessor navtrackContextAccessor) : BaseRequestHandler<CreateOrUpdateAssetDeviceRequest>
{
    private AssetEntity? asset;
    private DeviceType? deviceType;

    public override async Task Validate(RequestValidationContext<CreateOrUpdateAssetDeviceRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();

        deviceType = deviceTypeRepository.GetById(context.Request.Model.DeviceTypeId);
        context.ValidationException.AddErrorIfNull(
            deviceType, nameof(context.Request.Model.DeviceTypeId), ApiErrorCodes.Device_000001_DeviceTypeInvalid);

        context.ValidationException.AddErrorIfTrue(
            await deviceRepository.SerialNumberIsUsed(context.Request.Model.SerialNumber, deviceType.Protocol.Port,
                asset.Id),
            nameof(context.Request.Model.SerialNumber), ApiErrorCodes.Device_000002_SerialNumberUsed);
    }

    public override async Task Handle(CreateOrUpdateAssetDeviceRequest request)
    {
        List<DeviceEntity> devices = await deviceRepository.GetDevicesByAssetId(asset.Id);
        CreateOrUpdateAssetDeviceModel model = request.Model;

        DeviceEntity? existingDevice = devices.FirstOrDefault(x =>
            x.DeviceTypeId == model.DeviceTypeId && x.SerialNumber == model.SerialNumber);

        if (existingDevice != null)
        {
            await assetRepository.SetActiveDevice(asset!.Id, existingDevice.Id);
        }
        else
        {
            DeviceEntity newDevice = DeviceDocumentMapper.Map(asset!, navtrackContextAccessor.NavtrackContext.User.Id,
                model.SerialNumber, deviceType!);

            await deviceRepository.Add(newDevice);

            await assetRepository.SetActiveDevice(asset!.Id, newDevice.Id);
        }
    }
}