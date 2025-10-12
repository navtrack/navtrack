using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetListMapper
{
    public static Model.Common.ListModel<AssetModel> Map(IEnumerable<AssetEntity> source,
        IEnumerable<DeviceType> deviceTypes)
    {
        Model.Common.ListModel<AssetModel> list = new()
        {
            Items = source
                .Select(x =>
                {
                    DeviceType deviceType = deviceTypes.First(y => y.Id == x.Device?.DeviceTypeId.ToString());

                    return AssetMapper.Map(x, deviceType);
                })
                .ToList()
        };
        
        return list;
    }
}