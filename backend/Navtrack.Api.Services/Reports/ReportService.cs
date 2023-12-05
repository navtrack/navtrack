using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Mappers.Reports;
using Navtrack.Api.Services.Roles;
using Navtrack.Api.Services.Trips;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IReportService))]
public class ReportService(
    IAssetRepository repository,
    IRoleService service,
    ICurrentUserAccessor userAccessor,
    ITripService tripService)
    : IReportService
{
    public async Task<DistanceReportListModel> GetDistanceReport(string assetId,
        DistanceReportFilterModel distanceReportFilter)
    {
        AssetDocument asset = await repository.GetById(assetId);

        service.CheckRole(asset, AssetRoleType.Viewer);

        TripListModel trips = await tripService.GetTrips(assetId, new TripFilterModel
        {
            StartDate = distanceReportFilter.StartDate,
            EndDate = distanceReportFilter.EndDate
        });

        UserDocument user = await userAccessor.Get();

        DistanceReportListModel listModel = DistanceReportListModelMapper.Map(trips.Items, user.UnitsType);

        return listModel;
    }
}