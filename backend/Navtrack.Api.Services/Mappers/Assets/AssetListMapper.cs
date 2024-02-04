using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetListMapper
{
    public static ListModel<AssetModel> Map(UserDocument currentUser, IEnumerable<AssetDocument> source,
        IEnumerable<DeviceType> deviceTypes)
    {
        ListModel<AssetModel> list = new()
        {
            Items = source
                .Select(x =>
                {
                    DeviceType deviceType = deviceTypes.First(y => y.Id == x.Device.DeviceTypeId.ToString());

                    return AssetModelMapper.Map(currentUser, x, deviceType);
                })
                .ToList()
        };
        
        return list;
    }
}