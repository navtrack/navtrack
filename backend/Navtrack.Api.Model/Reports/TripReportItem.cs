using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Model.Reports;

public class TripReportItem
{
    [Required]
    public MessagePosition StartPosition { get; set; }

    [Required]
    public MessagePosition EndPosition { get; set; }

    public int Distance { get; set; }
    public double Duration { get; set; }
    public double? FuelConsumption { get; set; }
    public double? AverageFuelConsumption { get; set; }
    public double? MaxSpeed { get; set; }
    public double? AverageSpeed { get; set; }
}