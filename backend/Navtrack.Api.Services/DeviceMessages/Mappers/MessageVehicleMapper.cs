using Navtrack.Api.Model.Messages;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageVehicleMapper
{
    public static VehicleDataModel? Map(DeviceMessageEntity source)
    {
        if (source.VehicleOdometer.HasValue || source.VehicleIgnition.HasValue || source.VehicleVoltage.HasValue)
        {
            VehicleDataModel result = new()
            {
                Odometer = source.VehicleOdometer,
                Ignition = source.VehicleIgnition,
                Voltage = source.VehicleVoltage
            };

            return result;
        }

        return null;
    }
}