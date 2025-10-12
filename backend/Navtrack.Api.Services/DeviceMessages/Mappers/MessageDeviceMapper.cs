using Navtrack.Api.Model.Messages;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageDeviceMapper
{
    public static DeviceDataModel? Map(DeviceMessageEntity source)
    {
        if (source.DeviceOdometer.HasValue || source.DeviceBatteryLevel.HasValue ||
            source.DeviceBatteryVoltage.HasValue || source.DeviceBatteryCurrent.HasValue)
        {
            DeviceDataModel result = new()
            {
                Odometer = source.DeviceOdometer,
                BatteryLevel = source.DeviceBatteryLevel,
                BatteryVoltage = source.DeviceBatteryVoltage,
                BatteryCurrent = source.DeviceBatteryCurrent
            };

            return result;
        }

        return null;
    }
}