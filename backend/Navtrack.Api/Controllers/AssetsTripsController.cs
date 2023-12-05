using Navtrack.Api.Services.Trips;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsTripsController(ITripService tripService) : AssetsTripsControllerBase(tripService);