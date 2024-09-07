using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Stats;

public class AssetStatsItemModel
{
    [Required]
    public AssetStatsDateRange DateRange { get; set; }
    
    public int? Distance { get; set; }
    public int? DistancePrevious { get; set; }
    public int? DistanceChange { get; set; }

    public int? Duration { get; set; }
    public int? DurationPrevious { get; set; }
    public int? DurationChange { get; set; }
    
    public double? FuelConsumption { get; set; }
    public double? FuelConsumptionPrevious { get; set; }
    public int? FuelConsumptionChange { get; set; }
}