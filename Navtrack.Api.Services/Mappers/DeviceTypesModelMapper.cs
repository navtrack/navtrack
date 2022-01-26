using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers;

public class DeviceTypesModelMapper
{
    public static DeviceTypesModel Map(IEnumerable<DeviceType> deviceTypes)
    {
        return new DeviceTypesModel
        {
            Items = deviceTypes.Select(DeviceTypeModelMapper.Map)
                .OrderBy(x => x.DisplayName)
                .ToList()
        };
    }
}