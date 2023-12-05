using Navtrack.Api.Services.Trips;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsTripsController : AssetsTripsControllerBase
{
    public AssetsTripsController(ITripService tripService) : base(tripService)
    {
    }
}