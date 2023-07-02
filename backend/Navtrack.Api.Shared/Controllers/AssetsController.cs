using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Assets;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class AssetsController : ControllerBase
{
    private readonly IAssetService assetService;

    public AssetsController(IAssetService assetService)
    {
        this.assetService = assetService;
    }

    [HttpGet("assets")]
    [ProducesResponseType(typeof(AssetsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAssets()
    {
        AssetsModel assets = await assetService.GetAssets();

        return new JsonResult(assets);
    }

    [HttpGet("assets/{assetId}")]
    [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<AssetModel> GetAsset(string assetId)
    {
        AssetModel asset = await assetService.GetById(assetId);

        return asset;
    }

    [HttpPost("assets")]
    [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<AssetModel> AddAsset([FromBody] AddAssetModel model)
    {
        AssetModel asset = await assetService.Add(model);

        return asset;
    }

    [HttpPatch("assets/{assetId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> UpdateAsset(string assetId, [FromBody] UpdateAssetModel model)
    {
        await assetService.Update(assetId, model);

        return Ok();
    }

    [HttpDelete("assets/{assetId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> DeleteAsset(string assetId)
    {
        await assetService.Delete(assetId);

        return Ok();
    }
}