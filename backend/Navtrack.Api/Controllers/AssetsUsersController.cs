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
public class AssetsUsersController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.AssetUsers)]
    [ProducesResponseType(typeof(ListModel<AssetUserModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(AssetUserRole.Owner)]
    public Task<ListModel<AssetUserModel>> GetList([FromRoute] string assetId) =>
        Query<GetAssetUsersRequest, ListModel<AssetUserModel>>(new GetAssetUsersRequest
        {
            AssetId = assetId
        });

    [HttpPost(ApiPaths.AssetUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [NavtrackAuthorize(AssetUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string assetId, [FromBody] CreateAssetUserModel model) =>
        await Command(new CreateAssetUserRequest
        {
            AssetId = assetId,
            Model = model
        });

    [HttpDelete(ApiPaths.AssetUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [NavtrackAuthorize(AssetUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string assetId, [FromRoute] string userId) =>
        await Command(new DeleteAssetUserRequest
        {
            AssetId = assetId,
            UserId = userId
        });
}
