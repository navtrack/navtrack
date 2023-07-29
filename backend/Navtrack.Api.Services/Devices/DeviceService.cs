using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
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
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IDeviceService))]
public class DeviceService : IDeviceService
{
    private readonly IDeviceTypeDataService deviceTypeDataService;
    private readonly IDeviceDataService deviceDataService;
    private readonly IAssetDataService assetDataService;
    private readonly ILocationDataService locationDataService;
    private readonly ICurrentUserAccessor currentUserAccessor;

    public DeviceService(IDeviceTypeDataService deviceTypeDataService, IDeviceDataService deviceDataService,
        IAssetDataService assetDataService, ILocationDataService locationDataService,
        ICurrentUserAccessor currentUserAccessor)
    {
        this.deviceTypeDataService = deviceTypeDataService;
        this.deviceDataService = deviceDataService;
        this.assetDataService = assetDataService;
        this.locationDataService = locationDataService;
        this.currentUserAccessor = currentUserAccessor;
    }

    public Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string? excludeAssetId = null)
    {
        DeviceType deviceType = deviceTypeDataService.GetById(deviceTypeId);

        return deviceDataService.SerialNumberIsUsed(serialNumber, deviceType.Protocol.Port, excludeAssetId);
    }

    public async Task<DevicesModel> Get(string assetId)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);
        asset.Return404IfNull();

        List<DeviceDocument> devices = await deviceDataService.GetDevicesByAssetId(assetId);
        IEnumerable<DeviceType> deviceTypes = deviceTypeDataService.GetDeviceTypes()
            .Where(x => devices.Any(y => y.DeviceTypeId == x.Id))
            .ToList();

        Dictionary<ObjectId, int> locationCount =
            await locationDataService.GetLocationsCountByDeviceIds(devices.Select(x => x.Id));

        return DeviceListModelMapper.Map(devices, deviceTypes, locationCount, asset);
    }

    public async Task Change(string assetId, ChangeDeviceModel model)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);
        asset.Return404IfNull();

        if (asset.Device.DeviceTypeId == model.DeviceTypeId && asset.Device.SerialNumber == model.SerialNumber)
        {
            return;
        }

        if (!deviceTypeDataService.Exists(model.DeviceTypeId))
        {
            throw new ValidationException()
                .AddValidationError(nameof(model.DeviceTypeId), ValidationErrorCodes.DeviceTypeInvalid);
        }

        if (await SerialNumberIsUsed(model.SerialNumber, model.DeviceTypeId, assetId))
        {
            throw new ValidationException()
                .AddValidationError(nameof(model.SerialNumber), ValidationErrorCodes.SerialNumberAlreadyUsed);
        }

        List<DeviceDocument> devices = await deviceDataService.GetDevicesByAssetId(assetId);

        DeviceDocument? existingDevice = devices.FirstOrDefault(x =>
            x.DeviceTypeId == model.DeviceTypeId && x.SerialNumber == model.SerialNumber);
        DeviceType deviceType = deviceTypeDataService.GetById(model.DeviceTypeId);

        if (existingDevice != null)
        {
            await assetDataService.SetActiveDevice(asset.Id, existingDevice.Id, existingDevice.SerialNumber,
                existingDevice.DeviceTypeId, deviceType.Protocol.Port);
        }
        else
        {
            UserDocument currentUser = await currentUserAccessor.Get();
            DeviceDocument newDevice = DeviceDocumentMapper.Map(assetId, model, currentUser.Id);

            await deviceDataService.Add(newDevice);

            await assetDataService.SetActiveDevice(asset.Id, newDevice.Id, newDevice.SerialNumber,
                newDevice.DeviceTypeId, deviceType.Protocol.Port);
        }
    }

    public async Task Delete(string id)
    {
        if (await deviceDataService.IsActive(id))
        {
            throw new ValidationException(ValidationErrorCodes.DeviceIsActive);
        }
        
        if (await locationDataService.DeviceHasLocations(id))
        {
            throw new ValidationException(ValidationErrorCodes.DeviceIsActive);
        }

        await deviceDataService.Delete(id);
    }
}