using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Extensions;
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
    IMessageRepository messageRepository)
    : IDeviceService
{
    public Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string? excludeAssetId = null)
    {
        DeviceType deviceType = typeRepository.GetById(deviceTypeId);

        return repository.SerialNumberIsUsed(serialNumber, deviceType.Protocol.Port, excludeAssetId);
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
            await messageRepository.GetMessagesCountByDeviceIds(devices.Select(x => x.Id));

        return DeviceListModelMapper.Map(devices, deviceTypes, locationCount, asset);
    }

    public async Task CreateOrUpdate(string assetId, CreateOrUpdateAssetDeviceModel model)
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
                .AddValidationError(nameof(model.DeviceTypeId), ValidationErrorCodes.DeviceTypeInvalid);
        }

        if (await SerialNumberIsUsed(model.SerialNumber, model.DeviceTypeId, assetId))
        {
            throw new ValidationApiException()
                .AddValidationError(nameof(model.SerialNumber), ValidationErrorCodes.SerialNumberAlreadyUsed);
        }

        List<DeviceDocument> devices = await repository.GetDevicesByAssetId(assetId);

        DeviceDocument? existingDevice = devices.FirstOrDefault(x =>
            x.DeviceTypeId == model.DeviceTypeId && x.SerialNumber == model.SerialNumber);
        DeviceType deviceType = typeRepository.GetById(model.DeviceTypeId);

        if (existingDevice != null)
        {
            await assetRepository.SetActiveDevice(asset.Id, existingDevice.Id, existingDevice.SerialNumber,
                existingDevice.DeviceTypeId, deviceType.Protocol.Port);
        }
        else
        {
            UserDocument currentUser = await userAccessor.Get();
            DeviceDocument newDevice = DeviceDocumentMapper.Map(assetId, model, currentUser.Id);

            await repository.Add(newDevice);

            await assetRepository.SetActiveDevice(asset.Id, newDevice.Id, newDevice.SerialNumber,
                newDevice.DeviceTypeId, deviceType.Protocol.Port);
        }
    }

    public async Task Delete(string assetId, string deviceId)
    {
        if (await repository.IsActive(assetId, deviceId))
        {
            throw new ValidationApiException(ValidationErrorCodes.DeviceIsActive);
        }
        
        if (await messageRepository.DeviceHasMessages(assetId, deviceId))
        {
            throw new ValidationApiException(ValidationErrorCodes.DeviceIsActive);
        }

        await repository.Delete(deviceId);
    }
}