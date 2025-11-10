using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Reports;

public class DistanceReportModel : ListModel<DistanceReportItemModel>
{
    [Required]
    public double TotalDistance => Items.Sum(x => x.Distance);

    [Required]
    public double TotalDuration => Items.Sum(x => x.Duration);
    
    [Required]
    public double? AverageSpeed => Items.Any() ? Items.Average(x => x.AverageSpeed) : null;
    
    [Required]
    public double? MaxSpeed => Items.Any() ? Items.Max(x => x.MaxSpeed) : null;
}