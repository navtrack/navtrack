using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Listener.Services.Mappers;

public static class VehicleElementMapper
{
    public static VehicleElement? Map(VehicleElement? destination)
    {
        if (destination != null)
        {
            destination.Odometer = destination.Odometer == 0 ? null : destination.Odometer;
            destination.Ignition = destination.Ignition == false ? null : destination.Ignition;
            destination.Voltage = destination.Voltage == 0 ? null : destination.Voltage;
        }

        return destination == null || destination.IsNull() ? null : destination;
    }
}