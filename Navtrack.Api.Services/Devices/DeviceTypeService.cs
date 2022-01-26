using System.Collections.Generic;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Mappers;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IDeviceTypeService))]
public class DeviceTypeService : IDeviceTypeService
{
    private readonly IDeviceTypeDataService deviceTypeDataService;

    public DeviceTypeService(IDeviceTypeDataService deviceTypeDataService)
    {
        this.deviceTypeDataService = deviceTypeDataService;
    }

    public DeviceTypesModel GetAll()
    {
        IEnumerable<DeviceType> devicesTypes = deviceTypeDataService.GetDeviceTypes();

        return DeviceTypesModelMapper.Map(devicesTypes);
    }
}