using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams;
using Navtrack.DataAccess.Model.Teams;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.TeamsAssets)]
public class TeamsAssetsController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.TeamAssets)]
    [ProducesResponseType(typeof(List<TeamAsset>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeTeam(TeamUserRole.Member)]
    public async Task<List<TeamAsset>> List([FromRoute] string teamId)
    {
        List<TeamAsset> result =
            await requestHandler.Handle<GetTeamAssetsRequest, List<TeamAsset>>(
                new GetTeamAssetsRequest
                {
                    TeamId = teamId
                });

        return result;
    }

    [HttpPost(ApiPaths.TeamAssets)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeTeam(TeamUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string teamId, [FromBody] CreateTeamAsset model)
    {
        await requestHandler.Handle(new CreateTeamAssetRequest
            {
                TeamId = teamId,
                Model = model
            });

        return Ok();
    }

    [HttpDelete(ApiPaths.TeamAssetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeTeam(TeamUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string teamId, [FromRoute] string assetId)
    {
        await requestHandler.Handle(new DeleteTeamAssetRequest
            {
                TeamId = teamId,
                AssetId = assetId
            });

        return Ok();
    }
}