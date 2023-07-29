using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Mappers.Devices;
using Navtrack.Api.Services.Mappers.Locations;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetModelMapper
{
    public static AssetModel Map(AssetDocument asset, DeviceType deviceType, List<UserDocument>? users = null)
    {
        return new AssetModel
        {
            Id = asset.Id.ToString(),
            Name = asset.Name,
            Location = asset.Location != null ? LocationMapper.Map(asset.Location) : null,
            Device = DeviceModelMapper.Map(asset, deviceType),
            Users = users != null
                ? asset.UserRoles
                    .Select(x =>
                        AssetUserModelMapper.Map(x, users.First(y => x.UserId == y.Id)))
                    .ToList() : null
        };
    }
}