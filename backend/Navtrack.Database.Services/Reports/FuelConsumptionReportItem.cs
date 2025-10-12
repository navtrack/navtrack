using System;

namespace Navtrack.Database.Services.Reports;

public class FuelConsumptionReportItem
{
    public DateTime Date { get; set; }
    public double? AverageSpeed { get; set; }
    public double? Duration { get; set; }
    public double? Distance { get; set; }
    public double? FuelConsumption { get; set; }
}