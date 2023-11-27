using System.Net.Mime;
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
public abstract class AssetsDevicesControllerBase : ControllerBase
{
    private readonly IDeviceService deviceService;

    protected AssetsDevicesControllerBase(IDeviceService deviceService)
    {
        this.deviceService = deviceService;
    }

    [HttpGet(ApiPaths.AssetsAssetDevices)]
    [ProducesResponseType(typeof(ListModel<DeviceModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<JsonResult> GetDevices([FromRoute] string assetId)
    {
        ListModel<DeviceModel> model = await deviceService.Get(assetId);

        return new JsonResult(model);
    }

    [HttpPost(ApiPaths.AssetsAssetDevices)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> ChangeDevice([FromRoute] string assetId, [FromBody] ChangeDeviceModel model)
    {
        await deviceService.Change(assetId, model);

        return Ok();
    }

    [HttpDelete(ApiPaths.AssetsAssetDevicesDevice)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> DeleteDevice([FromRoute] string assetId, [FromRoute] string deviceId)
    {
        await deviceService.Delete(assetId, deviceId);

        return Ok();
    }
}