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

public class AssetsController(IAssetService service) : AssetsControllerBase(service)
{
    [HttpGet(ApiPaths.Assets)]
    [ProducesResponseType(typeof(ListModel<AssetModel>), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetList()
    {
        ListModel<AssetModel> assets = await service.GetAssets();

        return new JsonResult(assets);
    }
}