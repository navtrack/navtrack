using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Stats;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Stats;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class AssetsStatsControllerBase(IAssetStatsService assetStatsService) : ControllerBase
{
    [HttpGet(ApiPaths.AssetsAssetStats)]
    [ProducesResponseType(typeof(AssetStatsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<JsonResult> Get([FromRoute] string assetId)
    {
        AssetStatsListModel distanceReport = await assetStatsService.GetStats(assetId);

        return new JsonResult(distanceReport);
    }
}