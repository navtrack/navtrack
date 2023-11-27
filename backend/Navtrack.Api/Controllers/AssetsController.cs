using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsController : AssetsControllerBase
{
    private readonly IAssetService assetService;

    public AssetsController(IAssetService assetService) : base(assetService)
    {
        this.assetService = assetService;
    }

    [HttpGet(ApiPaths.Assets)]
    [ProducesResponseType(typeof(ListModel<AssetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAssets()
    {
        ListModel<AssetModel> assets = await assetService.GetAssets();

        return new JsonResult(assets);
    }
}