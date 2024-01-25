using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Trips;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class AssetsTripsControllerBase(ITripService tripService) : ControllerBase
{
    [HttpGet(ApiPaths.AssetsAssetTrips)]
    [ProducesResponseType(typeof(TripListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public virtual async Task<JsonResult> GetList([FromRoute] string assetId, [FromQuery] TripFilterModel filter)
    {
        TripListModel locations = await tripService.GetTrips(assetId, filter);

        return new JsonResult(locations);
    }
}