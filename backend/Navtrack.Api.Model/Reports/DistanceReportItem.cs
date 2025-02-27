using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Reports;

public class DistanceReportItem
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int? Distance { get; set; }

    [Required]
    public double? Duration { get; set; }

    public double? FuelConsumption { get; set; }

    public double? AverageFuelConsumption
    {
        get
        {
            if (Distance.HasValue && FuelConsumption.HasValue)
            {
                return FuelConsumption * 100 / Distance * 1000;
            }

            return null;
        }
    }
}