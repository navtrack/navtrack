using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;

namespace Navtrack.Api.Services.Reports;

public interface IReportService
{
    Task<DistanceReportListModel> GetDistanceReport(string assetId, DistanceReportFilterModel tripFilter);
}