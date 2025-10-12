using Navtrack.Api.Model.Devices;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceMapper
{
    public static DeviceModel Map(DeviceEntity device, DeviceType deviceType, AssetEntity? asset, int? locationCount = null)
    {
        return new DeviceModel
        {
            Id = device.Id.ToString(),
            SerialNumber = device.SerialNumber,
            DeviceType = DeviceTypeMapper.Map(deviceType),
            Active = asset?.Device?.Id == device.Id,
            Positions = locationCount
        };
    }

    public static DeviceModel? Map(DeviceEntity? device, DeviceType? deviceType)
    {
        if (device != null && deviceType != null)
        {
            return new DeviceModel
            {
                Id = device.Id.ToString(),
                SerialNumber = device.SerialNumber,
                DeviceType = DeviceTypeMapper.Map(deviceType),
                Active = true
            };
        }

        return null;
    }
}