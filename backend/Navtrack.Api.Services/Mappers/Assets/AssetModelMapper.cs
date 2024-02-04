using System;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Mappers.Devices;
using Navtrack.Api.Services.Mappers.Positions;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetModelMapper
{
    public static AssetModel Map(UserDocument currentUser, AssetDocument asset, DeviceType deviceType,
        AssetModel? destination = null)
    {
        bool isOwner = AssetAuthorize.UserHasAssetRole(currentUser, asset.Id.ToString(), AssetRoleType.Owner);
        
        AssetModel model = destination ?? new AssetModel();

        model.Id = asset.Id.ToString();
        model.Name = asset.Name;
        model.Position = asset.Position != null ? PositionMapper.Map(asset.Position) : null;
        model.Online = asset.Position?.CreatedDate > DateTime.UtcNow.AddMinutes(-1);
        model.Device = DeviceModelMapper.Map(asset, deviceType);
        model.UserRoles = asset.UserRoles
            .Where(x => isOwner || x.UserId == currentUser.Id)
            .Select(AssetUserRoleModelMapper.Map)
            .ToList();

        return model;
    }
}