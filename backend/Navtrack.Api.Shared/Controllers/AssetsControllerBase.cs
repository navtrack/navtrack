using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Assets;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class AssetsControllerBase(IAssetService service) : ControllerBase
{
    [HttpGet(ApiPaths.AssetsAsset)]
    [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<AssetModel> GetAsset(string assetId)
    {
        AssetModel asset = await service.GetById(assetId);

        return asset;
    }

    [HttpPost(ApiPaths.Assets)]
    [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<AssetModel> CreateAsset([FromBody] CreateAssetModel model)
    {
        AssetModel asset = await service.Create(model);

        return asset;
    }

    [HttpPatch(ApiPaths.AssetsAsset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> UpdateAsset(string assetId, [FromBody] UpdateAssetModel model)
    {
        await service.Update(assetId, model);

        return Ok();
    }

    [HttpDelete(ApiPaths.AssetsAsset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> DeleteAsset(string assetId)
    {
        await service.Delete(assetId);

        return Ok();
    }
}