using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Devices;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class AssetsDevicesController : ControllerBase
{
    private readonly IDeviceService deviceService;

    public AssetsDevicesController(IDeviceService deviceService)
    {
        this.deviceService = deviceService;
    }

    [HttpGet("assets/{assetId}/devices")]
    [ProducesResponseType(typeof(DevicesModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<JsonResult> GetDevices([FromRoute] string assetId)
    {
        DevicesModel model = await deviceService.Get(assetId);

        return new JsonResult(model);
    }

    [HttpPost("assets/{assetId}/devices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> AddDevice([FromRoute] string assetId, [FromBody] AddDeviceModel model)
    {
        await deviceService.Add(assetId, model);

        return Ok();
    }

    [HttpDelete("assets/{assetId}/devices/{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> DeleteDevice([FromRoute] string assetId, [FromRoute] string deviceId)
    {
        await deviceService.Delete(deviceId);

        return Ok();
    }
}