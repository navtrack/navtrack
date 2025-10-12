using Navtrack.Database.Model.Devices;

namespace Navtrack.Database.Services.Devices;

public class GetFirstAndLastPositionResult
{
    public DeviceMessageEntity? FirstOdometer { get; set; }
    public DeviceMessageEntity? LastOdometer { get; set; }
    public DeviceMessageEntity? FirstFuelConsumption { get; set; }
    public DeviceMessageEntity? LastFuelConsumption { get; set; }
}