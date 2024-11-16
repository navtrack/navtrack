using System;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.DeviceMessages.Mappers;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetMapper
{
    public static Asset Map(AssetDocument asset, DeviceType? deviceType, Asset? destination = null)
    {
        Asset model = destination ?? new Asset();

        model.Id = asset.Id.ToString();
        model.OrganizationId = asset.OrganizationId.ToString();
        model.Name = asset.Name;
        model.LastPositionMessage =
            asset.LastPositionMessage != null ? MessageMapper.Map(asset.LastPositionMessage) : null;
        model.LastMessage = asset.LastMessage != null ? MessageMapper.Map(asset.LastMessage) : null;
        model.Online = asset.LastMessage?.CreatedDate > DateTime.UtcNow.AddMinutes(-1);
        model.Device = DeviceMapper.Map(asset, deviceType);

        return model;
    }
}