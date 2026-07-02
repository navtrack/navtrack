using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers.Shared;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.Assets)]
public abstract class BaseAssetsController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpPost(ApiPaths.OrganizationAssets)]
    [ProducesResponseType(typeof(Entity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [NavtrackAuthorize(OrganizationUserRole.Owner)]
    public Task<Entity> Create([FromRoute] string organizationId, [FromBody] CreateAssetModel model) =>
        Query<CreateAssetRequest, Entity>(new CreateAssetRequest
        {
            OrganizationId = organizationId,
            Model = model
        });
    
    [HttpGet(ApiPaths.AssetById)]
    [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(AssetUserRole.Viewer)]
    public Task<AssetModel> Get([FromRoute] string assetId) =>
        Query<GetAssetRequest, AssetModel>(new GetAssetRequest
        {
            AssetId = assetId
        });

    [HttpPost(ApiPaths.AssetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(AssetUserRole.Owner)]
    public async Task<IActionResult> Update([FromRoute] string assetId, [FromBody] UpdateAssetModel model) =>
        await Command(new UpdateAssetRequest
        {
            AssetId = assetId,
            Model = model
        });

    [HttpDelete(ApiPaths.AssetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(AssetUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string assetId) =>
        await Command(new DeleteAssetRequest
        {
            AssetId = assetId
        });
}
