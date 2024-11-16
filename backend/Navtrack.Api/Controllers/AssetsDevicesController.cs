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
using Navtrack.DataAccess.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsDevices)]
public class AssetsDevicesController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.AssetDevices)]
    [ProducesResponseType(typeof(List<Device>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<List<Device>> GetList([FromRoute] string assetId)
    {
        List<Device> result =
            await requestHandler.Handle<GetAssetDevicesRequest, List<Device>>(
                new GetAssetDevicesRequest
                {
                    AssetId = assetId
                });

        return result;
    }

    [HttpPost(ApiPaths.AssetDevices)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<IActionResult> CreateOrUpdate([FromRoute] string assetId, [FromBody] CreateOrUpdateAssetDevice model)
    {
        await requestHandler.Handle(new CreateOrUpdateAssetDeviceRequest
        {
            AssetId = assetId,
            Model = model
        });

        return Ok();
    }

    [HttpDelete(ApiPaths.AssetDeviceById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string assetId, [FromRoute] string deviceId)
    {
        await requestHandler.Handle(new DeleteAssetDeviceRequest
        {
            AssetId = assetId,
            DeviceId = deviceId
        });

        return Ok();
    }
}
