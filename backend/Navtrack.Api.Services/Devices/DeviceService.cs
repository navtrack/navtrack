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
using Navtrack.DataAccess.Services.Locations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IDeviceService))]
public class DeviceService : IDeviceService
{
    private readonly IDeviceTypeRepository deviceTypeRepository;
    private readonly IDeviceRepository deviceRepository;
    private readonly IAssetRepository assetRepository;
    private readonly ILocationRepository locationRepository;
    private readonly ICurrentUserAccessor currentUserAccessor;

    public DeviceService(IDeviceTypeRepository deviceTypeRepository, IDeviceRepository deviceRepository,
        IAssetRepository assetRepository, ILocationRepository locationRepository,
        ICurrentUserAccessor currentUserAccessor)
    {
        this.deviceTypeRepository = deviceTypeRepository;
        this.deviceRepository = deviceRepository;
        this.assetRepository = assetRepository;
        this.locationRepository = locationRepository;
        this.currentUserAccessor = currentUserAccessor;
    }

    public Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string? excludeAssetId = null)
    {
        DeviceType deviceType = deviceTypeRepository.GetById(deviceTypeId);

        return deviceRepository.SerialNumberIsUsed(serialNumber, deviceType.Protocol.Port, excludeAssetId);
    }

    public async Task<ListModel<DeviceModel>> Get(string assetId)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        asset.Return404IfNull();

        List<DeviceDocument> devices = await deviceRepository.GetDevicesByAssetId(assetId);
        IEnumerable<DeviceType> deviceTypes = deviceTypeRepository.GetDeviceTypes()
            .Where(x => devices.Any(y => y.DeviceTypeId == x.Id))
            .ToList();

        Dictionary<ObjectId, int> locationCount =
            await locationRepository.GetLocationsCountByDeviceIds(devices.Select(x => x.Id));

        return DeviceListModelMapper.Map(devices, deviceTypes, locationCount, asset);
    }

    public async Task Change(string assetId, ChangeDeviceModel model)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);
        asset.Return404IfNull();

        if (asset.Device.DeviceTypeId == model.DeviceTypeId && asset.Device.SerialNumber == model.SerialNumber)
        {
            return;
        }

        if (!deviceTypeRepository.Exists(model.DeviceTypeId))
        {
            throw new ValidationException()
                .AddValidationError(nameof(model.DeviceTypeId), ValidationErrorCodes.DeviceTypeInvalid);
        }

        if (await SerialNumberIsUsed(model.SerialNumber, model.DeviceTypeId, assetId))
        {
            throw new ValidationException()
                .AddValidationError(nameof(model.SerialNumber), ValidationErrorCodes.SerialNumberAlreadyUsed);
        }

        List<DeviceDocument> devices = await deviceRepository.GetDevicesByAssetId(assetId);

        DeviceDocument? existingDevice = devices.FirstOrDefault(x =>
            x.DeviceTypeId == model.DeviceTypeId && x.SerialNumber == model.SerialNumber);
        DeviceType deviceType = deviceTypeRepository.GetById(model.DeviceTypeId);

        if (existingDevice != null)
        {
            await assetRepository.SetActiveDevice(asset.Id, existingDevice.Id, existingDevice.SerialNumber,
                existingDevice.DeviceTypeId, deviceType.Protocol.Port);
        }
        else
        {
            UserDocument currentUser = await currentUserAccessor.Get();
            DeviceDocument newDevice = DeviceDocumentMapper.Map(assetId, model, currentUser.Id);

            await deviceRepository.Add(newDevice);

            await assetRepository.SetActiveDevice(asset.Id, newDevice.Id, newDevice.SerialNumber,
                newDevice.DeviceTypeId, deviceType.Protocol.Port);
        }
    }

    public async Task Delete(string assetId, string deviceId)
    {
        if (await deviceRepository.IsActive(assetId, deviceId))
        {
            throw new ValidationException(ValidationErrorCodes.DeviceIsActive);
        }
        
        if (await locationRepository.DeviceHasLocations(assetId, deviceId))
        {
            throw new ValidationException(ValidationErrorCodes.DeviceIsActive);
        }

        await deviceRepository.Delete(deviceId);
    }
}