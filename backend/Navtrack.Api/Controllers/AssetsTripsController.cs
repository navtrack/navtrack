using Navtrack.Api.Services.Trips;

namespace Navtrack.Api.Controllers;

public class AssetsTripsController : Navtrack.Api.Shared.Controllers.AssetsTripsControllerBase
{
    public AssetsTripsController(ITripService tripService) : base(tripService)
    {
    }
}