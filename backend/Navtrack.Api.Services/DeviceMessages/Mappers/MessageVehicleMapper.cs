using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageVehicleMapper
{
    public static MessageVehicle? Map(VehicleElement? source)
    {
        if (source != null)
        {
            MessageVehicle result = new()
            {
                Odometer = source.Odometer,
                Ignition = source.Ignition,
                Voltage = source.Voltage
            };

            return result;
        }

        return null;
    }
}