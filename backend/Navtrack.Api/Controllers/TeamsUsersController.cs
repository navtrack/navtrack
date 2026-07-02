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
[OpenApiTag(ControllerTags.TeamsUsers)]
public class TeamsUsersController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.TeamUsers)]
    [ProducesResponseType(typeof(ListModel<TeamUserModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(TeamUserRole.Member)]
    public Task<ListModel<TeamUserModel>> List([FromRoute] string teamId) =>
        Query<GetTeamUsersRequest, ListModel<TeamUserModel>>(new GetTeamUsersRequest
        {
            TeamId = teamId
        });

    [HttpPost(ApiPaths.TeamUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(TeamUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string teamId, [FromBody] CreateTeamUserModel model) =>
        await Command(new CreateTeamUserRequest
        {
            TeamId = teamId,
            Model = model
        });

    [HttpPost(ApiPaths.TeamUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(TeamUserRole.Owner)]
    public async Task<IActionResult> Update([FromRoute] string teamId, [FromRoute] string userId,
        [FromBody] UpdateTeamUserModel model) =>
        await Command(new UpdateTeamUserRequest
        {
            TeamId = teamId,
            UserId = userId,
            Model = model
        });

    [HttpDelete(ApiPaths.TeamUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(TeamUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string teamId, [FromRoute] string userId) =>
        await Command(new DeleteTeamUserRequest
        {
            TeamId = teamId,
            UserId = userId
        });
}
