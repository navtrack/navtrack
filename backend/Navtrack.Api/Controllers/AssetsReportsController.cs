using Navtrack.Api.Services.Reports;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsReportsController : AssetsReportsControllerBase
{
    public AssetsReportsController(IReportService reportService) : base(reportService)
    {
    }
}