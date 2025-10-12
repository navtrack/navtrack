using Navtrack.Api.Model.Reports;

namespace Navtrack.Api.Services.Reports;

public class GetDistanceReportRequest
{
    public string AssetId { get; set; }
    public BaseReportFilterModel Model { get; set; }
}