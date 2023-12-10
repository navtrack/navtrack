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
    [Produces(MediaTypeNames.Application.Json)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<AssetModel> Get(string assetId)
    {
        AssetModel asset = await service.GetById(assetId);

        return asset;
    }

    [HttpPost(ApiPaths.Assets)]
    [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<AssetModel> Create([FromBody] CreateAssetModel model)
    {
        AssetModel asset = await service.Create(model);

        return asset;
    }

    [HttpPost(ApiPaths.AssetsAsset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> Update(string assetId, [FromBody] UpdateAssetModel model)
    {
        await service.Update(assetId, model);

        return Ok();
    }

    [HttpDelete(ApiPaths.AssetsAsset)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> Delete(string assetId)
    {
        await service.Delete(assetId);

        return Ok();
    }
}