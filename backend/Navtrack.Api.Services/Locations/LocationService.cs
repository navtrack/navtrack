using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Services.Mappers.Locations;
using Navtrack.Api.Services.Roles;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Locations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Locations;

[Service(typeof(ILocationService))]
public class LocationService : ILocationService
{
    private readonly IAssetRepository assetRepository;
    private readonly IRoleService roleService;
    private readonly ILocationRepository locationRepository;

    public LocationService(IAssetRepository assetRepository, IRoleService roleService,
        ILocationRepository locationRepository)
    {
        this.assetRepository = assetRepository;
        this.roleService = roleService;
        this.locationRepository = locationRepository;
    }

    public async Task<LocationListModel> GetLocations(string assetId, LocationFilterModel locationFilter, int page, int size)
    {
        AssetDocument asset = await assetRepository.GetById(assetId);

        roleService.CheckRole(asset, AssetRoleType.Viewer);

        List<LocationDocument> locations = await locationRepository.GetLocations(assetId, locationFilter);
    
        return LocationListMapper.Map(locations);
    }
}