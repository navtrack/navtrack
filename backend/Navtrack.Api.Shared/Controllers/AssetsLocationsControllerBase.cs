using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Locations;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class AssetsLocationsControllerBase(ILocationService service) : ControllerBase
{
    [HttpGet(ApiPaths.AssetsAssetLocations)]
    [ProducesResponseType(typeof(LocationListModel), StatusCodes.Status200OK)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<JsonResult> GetList(
        [FromRoute] string assetId,
        [FromQuery] LocationFilterModel filter,
        [FromQuery] int page = 0,
        [FromQuery] [Range(0, 1000)] int size = 1000)
    {
        LocationListModel locations = await service.GetLocations(assetId, filter, page, size);

        return new JsonResult(locations);
    }
}