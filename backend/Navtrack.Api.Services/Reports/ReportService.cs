using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Services.Mappers.Reports;
using Navtrack.Api.Services.Roles;
using Navtrack.Api.Services.User;
using Navtrack.Core.Model.Trips;
using Navtrack.Core.Services.Trips;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Locations;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IReportService))]
public class ReportService : IReportService
{
    private readonly IAssetDataService assetDataService;
    private readonly IRoleService roleService;
    private readonly ILocationDataService locationDataService;
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly ITripService tripService;

    public ReportService(IAssetDataService assetDataService, IRoleService roleService,
        ILocationDataService locationDataService, ICurrentUserAccessor currentUserAccessor, ITripService tripService)
    {
        this.assetDataService = assetDataService;
        this.roleService = roleService;
        this.locationDataService = locationDataService;
        this.currentUserAccessor = currentUserAccessor;
        this.tripService = tripService;
    }

    public async Task<DistanceReportListModel> GetDistanceReport(string assetId, DistanceReportFilterModel distanceReportFilter)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);

        roleService.CheckRole(asset, AssetRoleType.Viewer);

        IEnumerable<Trip> trips = await tripService.GetTrips(assetId, new TripFilter
        {
            StartDate = distanceReportFilter.StartDate,
            EndDate = distanceReportFilter.EndDate
        });
        
        UserDocument user = await currentUserAccessor.Get();

        DistanceReportListModel listModel = DistanceReportListModelMapper.Map(trips, user.UnitsType);

        return listModel;
    }
}