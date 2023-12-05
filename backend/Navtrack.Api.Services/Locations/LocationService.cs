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
public class LocationService(
    IAssetRepository repository,
    IRoleService service,
    ILocationRepository locationRepository)
    : ILocationService
{
    public async Task<LocationListModel> GetLocations(string assetId, LocationFilterModel locationFilter, int page, int size)
    {
        AssetDocument asset = await repository.GetById(assetId);

        service.CheckRole(asset, AssetRoleType.Viewer);

        List<LocationDocument> locations = await locationRepository.GetLocations(assetId, locationFilter);
    
        return LocationListMapper.Map(locations);
    }
}