using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams;
using Navtrack.Database.Model.Teams;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.TeamsAssets)]
public class TeamsAssetsController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.TeamAssets)]
    [ProducesResponseType(typeof(ListModel<TeamAssetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(TeamUserRole.Member)]
    public Task<ListModel<TeamAssetModel>> List([FromRoute] string teamId) =>
        Query<GetTeamAssetsRequest, ListModel<TeamAssetModel>>(new GetTeamAssetsRequest
        {
            TeamId = teamId
        });

    [HttpPost(ApiPaths.TeamAssets)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(TeamUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string teamId, [FromBody] CreateTeamAssetModel model) =>
        await Command(new CreateTeamAssetRequest
        {
            TeamId = teamId,
            Model = model
        });

    [HttpDelete(ApiPaths.TeamAssetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(TeamUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string teamId, [FromRoute] string assetId) =>
        await Command(new DeleteTeamAssetRequest
        {
            TeamId = teamId,
            AssetId = assetId
        });
}
