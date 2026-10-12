using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Reports;

public class DistanceReportItemModel
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public double Distance { get; set; }

    [Required]
    public double Duration { get; set; }
    
    [Required]
    public double AverageSpeed { get; set; }
    
    [Required]
    public double MaxSpeed { get; set; }

    public double? FuelConsumption { get; set; }

    public double? AverageFuelConsumption => FuelConsumption.HasValue && FuelConsumption.Value != 0 && Distance != 0
        ? Math.Round(FuelConsumption.Value * 100 / Distance * 1000, 2)
        : null;
}