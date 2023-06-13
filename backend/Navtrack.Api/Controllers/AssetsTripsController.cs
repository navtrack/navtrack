using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.Locations;
using Navtrack.Api.Services.Reports;
using Navtrack.Api.Services.Trips;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class AssetsTripsController : ControllerBase
{
    private readonly ITripService tripService;

    public AssetsTripsController(ITripService tripService)
    {
        this.tripService = tripService;
    }

    [HttpGet("assets/{assetId}/trips")]
    [ProducesResponseType(typeof(TripListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<JsonResult> GetTrips(
        [FromRoute] string assetId,
        [FromQuery] TripFilterModel filter)
    {
        TripListModel locations = await tripService.GetTrips(assetId, filter);

        return new JsonResult(locations);
    }
}