using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Devices;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class AssetsDevicesControllerBase(IDeviceService service) : ControllerBase
{
    [HttpGet(ApiPaths.AssetsAssetDevices)]
    [ProducesResponseType(typeof(ListModel<DeviceModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<JsonResult> GetList([FromRoute] string assetId)
    {
        ListModel<DeviceModel> model = await service.GetList(assetId);

        return new JsonResult(model);
    }

    [HttpPost(ApiPaths.AssetsAssetDevices)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> Update([FromRoute] string assetId, [FromBody] UpdateAssetDeviceModel model)
    {
        await service.Change(assetId, model);

        return Ok();
    }

    [HttpDelete(ApiPaths.AssetsAssetDevicesDevice)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string assetId, [FromRoute] string deviceId)
    {
        await service.Delete(assetId, deviceId);

        return Ok();
    }
}