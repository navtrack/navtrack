using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Reports;

public class FuelConsumptionReportItemModel
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public double? Distance { get; set; }

    [Required]
    public double? Duration { get; set; }

    public double? FuelConsumption { get; set; }

    public double? AverageFuelConsumption
    {
        get
        {
            if (Distance.HasValue && FuelConsumption.HasValue)
            {
                return Math.Round(FuelConsumption.Value * 100 / Distance.Value * 1000, 2);
            }

            return null;
        }
    }

    public double? AverageSpeed { get; set; }
}