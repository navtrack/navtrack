using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Reports;

public class DistanceReportListModel : ListModel<DistanceReportItemModel>
{
    [Required]
    public int TotalDistance => Items.Sum(x => x.Distance);

    [Required]
    public double TotalDuration => Items.Sum(x => x.Duration);
}