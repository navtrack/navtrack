using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers;

public class DeviceTypesModelMapper
{
    public static DeviceTypeListModel Map(IEnumerable<DeviceType> deviceTypes)
    {
        return new DeviceTypeListModel
        {
            Items = deviceTypes.Select(DeviceTypeModelMapper.Map)
                .OrderBy(x => x.DisplayName)
                .ToList()
        };
    }
}