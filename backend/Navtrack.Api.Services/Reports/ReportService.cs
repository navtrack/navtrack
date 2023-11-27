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
public class ReportService : IReportService
{
    private readonly IAssetRepository assetRepository;
    private readonly IRoleService roleService;
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly ITripService tripService;

    public ReportService(IAssetRepository assetRepository, IRoleService roleService,
        ICurrentUserAccessor currentUserAccessor, ITripService tripService)
    {
        this.assetRepository = assetRepository;
        this.roleService = roleService;
        this.currentUserAccessor = currentUserAccessor;
        this.tripService = tripService;
    }

    public async Task<DistanceReportListModel> GetDistanceReport(string assetId,
        DistanceReportFilterModel distanceReportFilter)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);

        roleService.CheckRole(asset, AssetRoleType.Viewer);

        TripListModel trips = await tripService.GetTrips(assetId, new TripFilterModel
        {
            StartDate = distanceReportFilter.StartDate,
            EndDate = distanceReportFilter.EndDate
        });

        UserDocument user = await currentUserAccessor.Get();

        DistanceReportListModel listModel = DistanceReportListModelMapper.Map(trips.Items, user.UnitsType);

        return listModel;
    }
}