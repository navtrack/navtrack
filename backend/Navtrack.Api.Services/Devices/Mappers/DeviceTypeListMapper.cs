using System.Collections.Generic;
using System.Linq;
using DeviceType = Navtrack.Api.Model.Devices.DeviceType;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceTypeListMapper
{
    public static Model.Common.List<DeviceType> Map(IEnumerable<DataAccess.Model.Devices.DeviceType> deviceTypes)
    {
        return new Model.Common.List<DeviceType>
        {
            Items = deviceTypes.Select(DeviceTypeMapper.Map)
                .OrderBy(x => x.DisplayName)
                .ToList()
        };
    }
}