using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Organizations;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers.Shared;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.Assets)]
public abstract class BaseAssetsController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpPost(ApiPaths.OrganizationAssets)]
    [ProducesResponseType(typeof(Entity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public Task<Entity> Create([FromRoute] string organizationId, [FromBody] CreateAsset model) =>
        requestHandler.Handle<CreateAssetRequest, Entity>(new CreateAssetRequest
        {
            OrganizationId = organizationId,
            Model = model
        });
    
    [HttpGet(ApiPaths.AssetById)]
    [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<Asset> Get([FromRoute] string assetId)
    {
        Asset result = await requestHandler.Handle<GetAssetRequest, Asset>(new GetAssetRequest
        {
            AssetId = assetId
        });
     
        return result;
    }

    [HttpPost(ApiPaths.AssetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<IActionResult> Update([FromRoute] string assetId, [FromBody] UpdateAsset model)
    {
        await requestHandler.Handle(new UpdateAssetRequest
        {
            AssetId = assetId,
            Model = model
        });

        return Ok();
    }

    [HttpDelete(ApiPaths.AssetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string assetId)
    {
        await requestHandler.Handle(new DeleteAssetRequest
        {
            AssetId = assetId
        });

        return Ok();
    }
}