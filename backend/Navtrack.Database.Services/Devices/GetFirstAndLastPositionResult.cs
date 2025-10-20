using Navtrack.Database.Model.Devices;

namespace Navtrack.Database.Services.Devices;

public class GetFirstAndLastPositionResult
{
    public int? FirstOdometer { get; set; }
    public int? LastOdometer { get; set; }
    public float? FirstFuelConsumption { get; set; }
    public float? LastFuelConsumption { get; set; }
}