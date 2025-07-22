using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Reports;

public class FuelConsumptionReport : List<FuelConsumptionReportItem>
{
    [Required]
    public double? TotalDistance => Items.Sum(x => x.Distance);

    [Required]
    public double? TotalDuration => Items.Sum(x => x.Duration);

    [Required]
    public double? TotalFuelConsumption => Items.Any(x => x.FuelConsumption != null) ? Items.Sum(x => x.FuelConsumption) : null;
    
    [Required]
    public double? AverageFuelConsumption => Items.Any(x => x.AverageFuelConsumption != null) ? Items.Average(x => x.AverageFuelConsumption) : null;
    
    [Required]
    public double? AverageSpeed => Items.Any(x => x.AverageSpeed != null) ? Items.Average(x => x.AverageSpeed) : null;
}