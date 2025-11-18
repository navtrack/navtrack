using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Reports;

public class FuelConsumptionReportItemModel
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int? Distance { get; set; }

    [Required]
    public int? Duration { get; set; }

    public double? FuelConsumption { get; set; }

    public double? AverageFuelConsumption
    {
        get
        {
            if (FuelConsumption.HasValue)
            {
                return Math.Round(FuelConsumption.Value * 100 / (double)Distance * 1000, 2);
            }

            return null;
        }
    }

    public double? AverageSpeed { get; set; }
}