using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Listener.Services.Mappers;

public static class DeviceElementMapper
{
    public static DeviceElement? Map(DeviceElement? destination)
    {
        if (destination != null)
        {
            destination.Odometer = destination.Odometer == 0 ? null : destination.Odometer;
            destination.BatteryLevel = destination.BatteryLevel == 0 ? null : destination.BatteryLevel;
            destination.BatteryVoltage = destination.BatteryVoltage == 0 ? null : destination.BatteryVoltage;
            destination.BatteryCurrent = destination.BatteryCurrent == 0 ? null : destination.BatteryCurrent;
        }

        return destination == null || destination.IsNull() ? null : destination;
    }
}