using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Services.Mappers;
using Navtrack.Api.Services.Roles;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
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

    public ReportService(IAssetDataService assetDataService, IRoleService roleService,
        ILocationDataService locationDataService, ICurrentUserAccessor currentUserAccessor)
    {
        this.assetDataService = assetDataService;
        this.roleService = roleService;
        this.locationDataService = locationDataService;
        this.currentUserAccessor = currentUserAccessor;
    }

    public async Task<DistanceReportListModel> GetDistanceReport(string assetId, DistanceReportFilterModel tripFilter)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);

        roleService.CheckRole(asset, AssetRoleType.Viewer);

        List<LocationDocument> locations =
            await locationDataService.GetLocations(assetId, tripFilter);

        UserDocument user = await currentUserAccessor.GetCurrentUser();

        DistanceReportListModel listModel = DistanceReportMapper.Map(locations, user.UnitsType);

        return listModel;
    }
}