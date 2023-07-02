using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Locations;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class AssetsLocationsController : ControllerBase
{
    private readonly ILocationService locationService;

    public AssetsLocationsController(ILocationService locationService)
    {
        this.locationService = locationService;
    }

    [HttpGet("assets/{assetId}/locations")]
    [ProducesResponseType(typeof(LocationListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<JsonResult> GetLocations(
        [FromRoute] string assetId,
        [FromQuery] LocationFilterModel filter,
        [FromQuery] int page = 0,
        [FromQuery] [Range(0, 1000)] int size = 1000)
    {
        LocationListModel locations = await locationService.GetLocations(assetId, filter, page, size);

        return new JsonResult(locations);
    }
}