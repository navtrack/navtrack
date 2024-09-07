using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceTypesModelMapper
{
    public static ListModel<DeviceTypeModel> Map(IEnumerable<DeviceType> deviceTypes)
    {
        return new ListModel<DeviceTypeModel>
        {
            Items = deviceTypes.Select(DeviceTypeModelMapper.Map)
                .OrderBy(x => x.DisplayName)
                .ToList()
        };
    }
}