using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageDeviceModelMapper
{
    public static MessageDeviceModel? Map(DeviceElement? source)
    {
        if (source != null)
        {
            MessageDeviceModel model = new()
            {
                Odometer = source.Odometer,
                BatteryLevel = source.BatteryLevel,
                BatteryVoltage = source.BatteryVoltage,
                BatteryCurrent = source.BatteryCurrent
            };

            return model;
        }

        return null;
    }
}