using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceListModelMapper
{
    public static ListModel<DeviceModel> Map(IEnumerable<DeviceDocument> devices, IEnumerable<DeviceType> deviceTypes,
        Dictionary<ObjectId, int> counts,
        AssetDocument assetDocument)
    {
        return new ListModel<DeviceModel>
        {
            Items = devices.Select(x =>
                DeviceModelMapper.Map(x, deviceTypes.First(d => d.Id == x.DeviceTypeId.ToString()), assetDocument,
                    counts.TryGetValue(x.Id, out int value) ? value : 0)).ToList()
        };
    }
}