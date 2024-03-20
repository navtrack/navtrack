using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers.Devices;

public static class DeviceModelMapper
{
    public static DeviceModel Map(DeviceDocument deviceDocument, DeviceType deviceType, AssetDocument assetDocument,
        int? locationCount = null)
    {
        return new DeviceModel
        {
            Id = deviceDocument.Id.ToString(),
            SerialNumber = deviceDocument.SerialNumber,
            DeviceType = DeviceTypeModelMapper.Map(deviceType),
            Active = assetDocument.Device?.Id == deviceDocument.Id,
            Positions = locationCount
        };
    }

    public static DeviceModel? Map(AssetDocument asset, DeviceType deviceType)
    {
        if (asset.Device != null)
        {
            return new DeviceModel
            {
                Id = asset.Device.Id.ToString(),
                SerialNumber = asset.Device.SerialNumber,
                DeviceType = DeviceTypeModelMapper.Map(deviceType),
                Active = true
            };
        }

        return null;
    }
}