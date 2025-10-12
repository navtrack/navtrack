using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Devices;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceTypeListMapper
{
    public static Model.Common.ListModel<DeviceTypeModel> Map(IEnumerable<DeviceType> deviceTypes)
    {
        return new Model.Common.ListModel<DeviceTypeModel>
        {
            Items = deviceTypes.Select(DeviceTypeMapper.Map)
                .OrderBy(x => x.DisplayName)
                .ToList()
        };
    }
}