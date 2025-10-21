namespace Navtrack.Database.Services.Devices;

public class GetFirstAndLastPositionResult
{
    public int? FirstIgnitionDuration { get; set; }
    public int? LastIgnitionDuration { get; set; }
    public int? FirstOdometer { get; set; }
    public int? LastOdometer { get; set; }
    public float? FirstFuelConsumption { get; set; }
    public float? LastFuelConsumption { get; set; }
}