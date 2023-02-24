using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetListMapper
{
    public static AssetsModel Map(IEnumerable<AssetDocument> source, UnitsType unitsType,
        IEnumerable<DeviceType> deviceTypes)
    {
        AssetsModel list = new()
        {
            Items = source
                .Select(x =>
                {
                    DeviceType deviceType = deviceTypes.First(y => y.Id == x.Device.DeviceTypeId.ToString());

                    return AssetModelMapper.Map(x, unitsType, deviceType);
                })
                .ToList()
        };
        return list;
    }
}