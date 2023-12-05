using Navtrack.Api.Services.Reports;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsReportsController(IReportService reportService) : AssetsReportsControllerBase(reportService);