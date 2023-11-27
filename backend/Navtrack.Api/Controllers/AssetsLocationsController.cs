using Navtrack.Api.Services.Locations;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsLocationsController : AssetsLocationsControllerBase
{
    public AssetsLocationsController(ILocationService locationService) : base(locationService)
    {
    }
}