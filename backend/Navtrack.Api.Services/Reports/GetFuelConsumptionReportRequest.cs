using Navtrack.Api.Model.Reports;

namespace Navtrack.Api.Services.Reports;

public class GetFuelConsumptionReportRequest
{
    public string AssetId { get; set; }
    public BaseReportFilter Model { get; set; }
}