namespace Navtrack.Api.Model.Stats;

public class AssetStatsModel
{
    public int? Distance { get; set; }
    public int? DistancePrevious { get; set; }

    public int? Duration { get; set; }
    public int? DurationPrevious { get; set; }
    
    public double? FuelConsumption { get; set; }
    public double? FuelConsumptionPrevious { get; set; }
    
    public double? FuelConsumptionAverage { get; set; }
    public double? FuelConsumptionAveragePrevious { get; set; }
}