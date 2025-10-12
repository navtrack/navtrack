using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Model.Reports;

public class TripReportItemModel
{
    [Required]
    public PositionDataModel StartPosition { get; set; }

    [Required]
    public PositionDataModel EndPosition { get; set; }

    public int Distance { get; set; }
    public double Duration { get; set; }
    public double? FuelConsumption { get; set; }
    public double? AverageFuelConsumption { get; set; }
    public double? MaxSpeed { get; set; }
    public double? AverageSpeed { get; set; }
}