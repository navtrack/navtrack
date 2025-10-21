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
using Navtrack.Database.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsStats)]
public class AssetsStatsController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.AssetStats)]
    [ProducesResponseType(typeof(AssetStatsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<AssetStatsModel> Get([FromRoute] string assetId, [FromRoute] AssetStatsPeriod period)
    {
        AssetStatsModel result =
            await requestHandler.Handle<GetAssetStatsRequest, AssetStatsModel>(new GetAssetStatsRequest
            {
                AssetId = assetId,
                Period = period
            });

        return result;
    }
}