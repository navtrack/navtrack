using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Stats;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Stats;
using Navtrack.DataAccess.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsStats)]
public class AssetsStatsController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.AssetStats)]
    [ProducesResponseType(typeof(AssetStatList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<AssetStatList> Get([FromRoute] string assetId)
    {
        AssetStatList result =
            await requestHandler.Handle<GetAssetStatsRequest, AssetStatList>(new GetAssetStatsRequest
            {
                AssetId = assetId
            });

        return result;
    }
}