using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsUsers)]
public class AssetsUsersController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.AssetUsers)]
    [ProducesResponseType(typeof(ListModel<AssetUserModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<ListModel<AssetUserModel>> GetList([FromRoute] string assetId)
    {
        ListModel<AssetUserModel> result =
            await requestHandler.Handle<GetAssetUsersRequest, ListModel<AssetUserModel>>(
                new GetAssetUsersRequest
                {
                    AssetId = assetId
                });

        return result;
    }

    [HttpPost(ApiPaths.AssetUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string assetId, [FromBody] CreateAssetUserModel model)
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
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
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
