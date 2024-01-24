using System;
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
    public static AssetModel Map(AssetDocument asset, DeviceType deviceType, List<UserDocument>? users = null,
        AssetModel? destination = null)
    {
        AssetModel model = destination ?? new AssetModel();
        
        model.Id = asset.Id.ToString();
        model.Name = asset.Name;
        model.Location = asset.Position != null ? LocationMapper.Map(asset.Position) : null;
        model.Online = asset.Location?.CreatedDate > DateTime.UtcNow.AddMinutes(-1);
        model.Device = DeviceModelMapper.Map(asset, deviceType);
        model.Users = users != null
            ? asset.UserRoles
                .Select(x =>
                    AssetUserModelMapper.Map(x, users.First(y => x.UserId == y.Id)))
                .ToList()
            : null;

        return model;
    }
}