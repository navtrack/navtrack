using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Stats;

public class AssetStatsItemModel
{
    [Required]
    public AssetStatsDateRange DateRange { get; set; }
    public int? Distance { get; set; }
    public int? PreviousDistance { get; set; }
    public int? DistanceChange { get; set; }

    public int? FuelConsumption { get; set; }
    public int? PreviousFuelConsumption { get; set; }
    public int? FuelConsumptionChange { get; set; }
    
    public int? Duration { get; set; }
    public int? PreviousDuration { get; set; }
    public int? DurationChange { get; set; }
}