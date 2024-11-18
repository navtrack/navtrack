using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.DataAccess.Services.Devices;

public class GetFirstAndLastPositionResult
{
    public DeviceMessageDocument? FirstOdometer { get; set; }
    public DeviceMessageDocument? LastOdometer { get; set; }
    public DeviceMessageDocument? FirstFuelConsumed { get; set; }
    public DeviceMessageDocument? LastFuelConsumed { get; set; }
}