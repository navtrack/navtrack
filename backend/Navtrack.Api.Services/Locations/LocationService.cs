using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Services.Mappers.Locations;
using Navtrack.Api.Services.Roles;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Model.New;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Locations;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Locations;

[Service(typeof(ILocationService))]
public class LocationService(
    IAssetRepository repository,
    IRoleService service,
    IPositionRepository locationRepository)
    : ILocationService
{
    public async Task<PositionListModel> GetLocations(string assetId, LocationFilterModel locationFilter, int page, int size)
    {
        AssetDocument asset = await repository.GetById(assetId);

        service.CheckRole(asset, AssetRoleType.Viewer);

        List<PositionGroupDocument> locations = await locationRepository.GetLocations(assetId, locationFilter);

        var list = locations.SelectMany(x => x.Positions).ToList();
        
        var mapped = LocationListMapper.Map(list);
    
        return mapped;
    }
}