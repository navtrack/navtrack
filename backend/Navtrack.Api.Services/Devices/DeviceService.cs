using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Extensions;
using Navtrack.Api.Services.Mappers.Assets;
using Navtrack.Api.Services.Mappers.Devices;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IDeviceService))]
public class DeviceService(
    IDeviceTypeRepository typeRepository,
    IDeviceRepository repository,
    IAssetRepository assetRepository,
    ICurrentUserAccessor userAccessor,
    IDeviceMessageRepository deviceMessageRepository)
    : IDeviceService
{
    public async Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string? excludeAssetId = null)
    {
        DeviceType? deviceType = typeRepository.GetById(deviceTypeId);

        return deviceType != null && await repository.SerialNumberIsUsed(serialNumber, deviceType.Protocol.Port, excludeAssetId);
    }

    public async Task<ListModel<DeviceModel>> GetList(string assetId)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        asset.Return404IfNull();

        List<DeviceDocument> devices = await repository.GetDevicesByAssetId(assetId);
        IEnumerable<DeviceType> deviceTypes = typeRepository.GetDeviceTypes()
            .Where(x => devices.Any(y => y.DeviceTypeId == x.Id))
            .ToList();

        Dictionary<ObjectId, int> locationCount =
            await deviceMessageRepository.GetMessagesCountByDeviceIds(devices.Select(x => x.Id));

        return DeviceListModelMapper.Map(devices, deviceTypes, locationCount, asset);
    }

    public async Task CreateOrUpdate(string assetId, UpdateAssetDeviceModel model)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        asset.Return404IfNull();

        if (asset.Device?.DeviceTypeId == model.DeviceTypeId && asset.Device.SerialNumber == model.SerialNumber)
        {
            return;
        }

        if (!typeRepository.Exists(model.DeviceTypeId))
        {
            throw new ValidationApiException()
                .AddValidationError(nameof(model.DeviceTypeId), ApiErrorCodes.DeviceTypeInvalid);
        }

        if (await SerialNumberIsUsed(model.SerialNumber, model.DeviceTypeId, assetId))
        {
            throw new ValidationApiException()
                .AddValidationError(nameof(model.SerialNumber), ApiErrorCodes.SerialNumberAlreadyUsed);
        }

        List<DeviceDocument> devices = await repository.GetDevicesByAssetId(assetId);

        DeviceDocument? existingDevice = devices.FirstOrDefault(x =>
            x.DeviceTypeId == model.DeviceTypeId && x.SerialNumber == model.SerialNumber);
        DeviceType deviceType = typeRepository.GetById(model.DeviceTypeId);

        if (existingDevice != null)
        {
            AssetDeviceElement assetDeviceElement = AssetDeviceElementMapper.Map(existingDevice, deviceType);

            await assetRepository.SetActiveDevice(asset.Id, assetDeviceElement);
        }
        else
        {
            UserDocument currentUser = await userAccessor.Get();
            DeviceDocument newDevice = DeviceDocumentMapper.Map(assetId, model, currentUser.Id);
       
            await repository.Add(newDevice);
            
            AssetDeviceElement assetDeviceElement = AssetDeviceElementMapper.Map(newDevice, deviceType);

            await assetRepository.SetActiveDevice(asset.Id, assetDeviceElement);
        }
    }

    public async Task Delete(string assetId, string deviceId)
    {
        if (await repository.IsActive(assetId, deviceId))
        {
            throw new ApiException(ApiErrorCodes.DeviceIsActive);
        }

        if (await deviceMessageRepository.DeviceHasMessages(assetId, deviceId))
        {
            throw new ApiException(ApiErrorCodes.DeviceIsActive);
        }

        await repository.Delete(deviceId);
    }
}