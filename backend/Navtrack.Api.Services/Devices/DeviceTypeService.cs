using System.Collections.Generic;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Mappers.Devices;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IDeviceTypeService))]
public class DeviceTypeService : IDeviceTypeService
{
    private readonly IDeviceTypeRepository deviceTypeRepository;

    public DeviceTypeService(IDeviceTypeRepository deviceTypeRepository)
    {
        this.deviceTypeRepository = deviceTypeRepository;
    }

    public ListModel<DeviceTypeModel> GetAll()
    {
        IEnumerable<DeviceType> devicesTypes = deviceTypeRepository.GetDeviceTypes();

        return DeviceTypesModelMapper.Map(devicesTypes);
    }
}