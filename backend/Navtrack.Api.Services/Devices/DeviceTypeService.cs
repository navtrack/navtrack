using System.Collections.Generic;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Mappers.Devices;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IDeviceTypeService))]
public class DeviceTypeService(IDeviceTypeRepository typeRepository) : IDeviceTypeService
{
    public ListModel<DeviceTypeModel> GetAll()
    {
        IEnumerable<DeviceType> devicesTypes = typeRepository.GetDeviceTypes();

        return DeviceTypesModelMapper.Map(devicesTypes);
    }
}