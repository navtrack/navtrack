using Navtrack.Api.Services.Locations;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsLocationsController(ILocationService locationService)
    : AssetsLocationsControllerBase(locationService);