namespace Navtrack.Api.Model.Stats;

public class AssetStatsModel
{
    public int? Distance { get; set; }
    public int? DistancePrevious { get; set; }
    public int? DistanceChange { get; set; }

    public int? Duration { get; set; }
    public int? DurationPrevious { get; set; }
    public int? DurationChange { get; set; }
    
    public double? FuelConsumption { get; set; }
    public double? FuelConsumptionPrevious { get; set; }
    public int? FuelConsumptionChange { get; set; }
    
    public double? FuelConsumptionAverage { get; set; }
    public double? FuelConsumptionAveragePrevious { get; set; }
    public int? FuelConsumptionAverageChange { get; set; }
}