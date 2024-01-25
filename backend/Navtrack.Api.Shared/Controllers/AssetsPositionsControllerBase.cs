using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Positions;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Positions;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class AssetsPositionsControllerBase(IPositionService service) : ControllerBase
{
    [HttpGet(ApiPaths.AssetsAssetPositions)]
    [ProducesResponseType(typeof(PositionListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<JsonResult> GetList(
        [FromRoute] string assetId,
        [FromQuery] PositionFilterModel filter,
        [FromQuery] int page = 0,
        [FromQuery] [Range(0, 1000)] int size = 1000)
    {
        PositionListModel positions = await service.GetPositions(assetId, filter, page, size);

        return new JsonResult(positions);
    }
}