using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageDeviceMapper
{
    public static MessageDevice? Map(DeviceElement? source)
    {
        if (source != null)
        {
            MessageDevice result = new()
            {
                Odometer = source.Odometer,
                BatteryLevel = source.BatteryLevel,
                BatteryVoltage = source.BatteryVoltage,
                BatteryCurrent = source.BatteryCurrent
            };

            return result;
        }

        return null;
    }
}