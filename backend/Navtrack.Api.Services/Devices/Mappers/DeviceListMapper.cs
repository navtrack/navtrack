using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using DeviceType = Navtrack.DataAccess.Model.Devices.DeviceType;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceListMapper
{
    public static Model.Common.List<Device> Map(IEnumerable<DeviceDocument> devices,
        IEnumerable<DeviceType> deviceTypes, Dictionary<ObjectId, int> counts, AssetDocument? assetDocument)
    {
        return new Model.Common.List<Device>
        {
            Items = devices.Select(x =>
                DeviceMapper.Map(x, deviceTypes.First(d => d.Id == x.DeviceTypeId.ToString()), assetDocument,
                    counts.GetValueOrDefault(x.Id, 0))).ToList()
        };
    }
}