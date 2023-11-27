using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetListMapper
{
    public static ListModel<AssetModel> Map(IEnumerable<AssetDocument> source, UnitsType unitsType,
        IEnumerable<DeviceType> deviceTypes)
    {
        ListModel<AssetModel> list = new()
        {
            Items = source
                .Select(x =>
                {
                    DeviceType deviceType = deviceTypes.First(y => y.Id == x.Device.DeviceTypeId.ToString());

                    return AssetModelMapper.Map(x, deviceType);
                })
                .ToList()
        };
        
        return list;
    }
}