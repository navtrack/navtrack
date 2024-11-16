using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using DeviceType = Navtrack.DataAccess.Model.Devices.DeviceType;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceMapper
{
    public static Device Map(DeviceDocument deviceDocument, DeviceType deviceType, AssetDocument? assetDocument,
        int? locationCount = null)
    {
        return new Device
        {
            Id = deviceDocument.Id.ToString(),
            SerialNumber = deviceDocument.SerialNumber,
            DeviceType = DeviceTypeMapper.Map(deviceType),
            Active = assetDocument?.Device?.Id == deviceDocument.Id,
            Positions = locationCount
        };
    }

    public static Device? Map(AssetDocument? asset, DeviceType? deviceType)
    {
        if (asset?.Device != null && deviceType != null)
        {
            return new Device
            {
                Id = asset.Device.Id.ToString(),
                SerialNumber = asset.Device.SerialNumber,
                DeviceType = DeviceTypeMapper.Map(deviceType),
                Active = true
            };
        }

        return null;
    }
}