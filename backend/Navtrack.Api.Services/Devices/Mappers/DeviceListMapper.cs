using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Devices;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceListMapper
{
    public static Model.Common.ListModel<DeviceModel> Map(List<DeviceEntity> devices,
        IEnumerable<DeviceType> deviceTypes, Dictionary<Guid, int> counts, AssetEntity assetDocument)
    {
        return new Model.Common.ListModel<DeviceModel>
        {
            Items = devices.Select(x =>
                DeviceMapper.Map(x, deviceTypes.First(d => d.Id == x.DeviceTypeId.ToString()), assetDocument,
                    counts.GetValueOrDefault(x.Id, 0))).ToList()
        };
    }
}