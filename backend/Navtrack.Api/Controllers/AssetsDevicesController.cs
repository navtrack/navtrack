using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsDevices)]
public class AssetsDevicesController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.AssetDevices)]
    [ProducesResponseType(typeof(ListModel<DeviceModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(AssetUserRole.Owner)]
    public Task<ListModel<DeviceModel>> GetList([FromRoute] string assetId) =>
        Query<GetAssetDevicesRequest, ListModel<DeviceModel>>(new GetAssetDevicesRequest
        {
            AssetId = assetId
        });

    [HttpPost(ApiPaths.AssetDevices)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [NavtrackAuthorize(AssetUserRole.Owner)]
    public async Task<IActionResult> CreateOrUpdate([FromRoute] string assetId, [FromBody] CreateOrUpdateAssetDeviceModel model) =>
        await Command(new CreateOrUpdateAssetDeviceRequest
        {
            AssetId = assetId,
            Model = model
        });

    [HttpDelete(ApiPaths.AssetDeviceById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [NavtrackAuthorize(AssetUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string assetId, [FromRoute] string deviceId) =>
        await Command(new DeleteAssetDeviceRequest
        {
            AssetId = assetId,
            DeviceId = deviceId
        });
}
