using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class VehicleModelMapper
{
    public static MessageVehicleModel? Map(VehicleElement? source)
    {
        if (source != null)
        {
            MessageVehicleModel model = new()
            {
                Odometer = source.Odometer,
                Ignition = source.Ignition,
                Voltage = source.Voltage
            };

            return model;
        }

        return null;
    }
}