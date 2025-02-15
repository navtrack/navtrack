using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsUsers)]
public class AssetsUsersController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.AssetUsers)]
    [ProducesResponseType(typeof(List<AssetUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<List<AssetUser>> GetList([FromRoute] string assetId)
    {
        List<AssetUser> result =
            await requestHandler.Handle<GetAssetUsersRequest, List<AssetUser>>(
                new GetAssetUsersRequest
                {
                    AssetId = assetId
                });

        return result;
    }

    [HttpPost(ApiPaths.AssetUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string assetId, [FromBody] CreateAssetUser model)
    {
        await requestHandler.Handle(new CreateAssetUserRequest
        {
            AssetId = assetId,
            Model = model
        });

        return Ok();
    }

    [HttpDelete(ApiPaths.AssetUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string assetId, [FromRoute] string userId)
    {
        await requestHandler.Handle(new DeleteAssetUserRequest
        {
            AssetId = assetId,
            UserId = userId
        });

        return Ok();
    }
}
