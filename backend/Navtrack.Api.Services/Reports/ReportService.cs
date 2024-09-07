using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Reports.Mappers;
using Navtrack.Api.Services.Trips;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IReportService))]
public class ReportService(ITripService tripService) : IReportService
{
    public async Task<DistanceReportListModel> GetDistanceReport(string assetId,
        DistanceReportFilterModel distanceReportFilter)
    {
        TripListModel trips = await tripService.GetTrips(assetId, new TripFilterModel
        {
            StartDate = distanceReportFilter.StartDate,
            EndDate = distanceReportFilter.EndDate
        });

        DistanceReportListModel list = DistanceReportListModelMapper.Map(trips.Items);

        return list;
    }
}