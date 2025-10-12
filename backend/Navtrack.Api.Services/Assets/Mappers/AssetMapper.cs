using System;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.DeviceMessages.Mappers;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetMapper
{
    public static AssetModel Map(AssetEntity asset, DeviceType? deviceType, AssetModel? destination = null)
    {
        AssetModel model = destination ?? new AssetModel();

        model.Id = asset.Id.ToString();
        model.OrganizationId = asset.OrganizationId.ToString();
        model.Name = asset.Name;
        model.LastPositionMessage =
            asset.LastPositionMessage != null ? MessageMapper.Map(asset.LastPositionMessage) : null;
        model.LastMessage = asset.LastMessage != null ? MessageMapper.Map(asset.LastMessage) : null;
        model.Online = asset.LastMessage?.CreatedDate > DateTime.UtcNow.AddMinutes(-1);
        model.Device = DeviceMapper.Map(asset.Device, deviceType);

        return model;
    }
}