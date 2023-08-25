using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Services.Mappers.Locations;
using Navtrack.Api.Services.Roles;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
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

    public LocationService(IAssetDataService assetDataService, IRoleService roleService,
        ILocationDataService locationDataService)
    {
        this.assetDataService = assetDataService;
        this.roleService = roleService;
        this.locationDataService = locationDataService;
    }

    public async Task<LocationListModel> GetLocations(string assetId, LocationFilterModel locationFilter, int page, int size)
    {
        AssetDocument asset = await assetDataService.GetById(assetId);

        roleService.CheckRole(asset, AssetRoleType.Viewer);

        List<LocationDocument> locations = await locationDataService.GetLocations(assetId, locationFilter);
    
        return LocationListMapper.Map(locations);
    }
}