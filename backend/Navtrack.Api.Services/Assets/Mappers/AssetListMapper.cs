using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetListMapper
{
    public static Model.Common.List<Asset> Map(IEnumerable<AssetDocument?> source,
        IEnumerable<DeviceType> deviceTypes)
    {
        Model.Common.List<Asset> list = new()
        {
            Items = source
                .Select(x =>
                {
                    DeviceType deviceType = deviceTypes.First(y => y.Id == x.Device.DeviceTypeId.ToString());

                    return AssetMapper.Map(x, deviceType);
                })
                .ToList()
        };
        
        return list;
    }
}