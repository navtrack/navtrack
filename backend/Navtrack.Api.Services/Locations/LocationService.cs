using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Services.Mappers;
using Navtrack.Api.Services.Roles;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Locations;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Locations;

[Service(typeof(ILocationService))]
public class LocationService : ILocationService
{
    private readonly IAssetDataService assetDataService;
    private readonly IRoleService roleService;
    private readonly ILocationDataService locationDataService;
    private readonly ICurrentUserAccessor currentUserAccessor;

    public LocationService(IAssetDataService assetDataService, IRoleService roleService,
        ILocationDataService locationDataService, ICurrentUserAccessor currentUserAccessor)
    {
        this.assetDataService = assetDataService;
        this.roleService = roleService;
        this.locationDataService = locationDataService;
        this.currentUserAccessor = currentUserAccessor;
    }

    public async Task<LocationListModel> GetLocations(string assetId, LocationFilterModel locationFilter, int page, int size)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);

        roleService.CheckRole(asset, AssetRoleType.Viewer);

        List<LocationDocument> locations = await locationDataService.GetLocations(assetId, locationFilter);
        UserDocument user = await currentUserAccessor.GetCurrentUser();

        return LocationListMapper.Map(locations, user.UnitsType);
    }
}