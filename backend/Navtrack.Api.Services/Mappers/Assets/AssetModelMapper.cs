using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Mappers.Devices;
using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetModelMapper
{
    public static AssetModel Map(AssetDocument asset, UnitsType unitsType, DeviceType deviceType)
    {
        return new AssetModel
        {
            Id = asset.Id.ToString(),
            Name = asset.Name,
            Location = asset.Location != null ? LocationMapper.Map(asset.Location, unitsType) : null,
            Device = DeviceModelMapper.Map(asset, deviceType)
        };
    }
}