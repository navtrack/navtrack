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
using Navtrack.DataAccess.Model.Teams;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.TeamsUsers)]
public class TeamsUsersController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.TeamUsers)]
    [ProducesResponseType(typeof(List<TeamUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeTeam(TeamUserRole.Member)]
    public async Task<List<TeamUser>> List([FromRoute] string teamId)
    {
        List<TeamUser> result = await requestHandler.Handle<GetTeamUsersRequest, List<TeamUser>>(
            new GetTeamUsersRequest
            {
                TeamId = teamId
            });

        return result;
    }

    [HttpPost(ApiPaths.TeamUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeTeam(TeamUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string teamId, [FromBody] CreateTeamUser model)
    {
        await requestHandler.Handle(new CreateTeamUserRequest
        {
            TeamId = teamId,
            Model = model
        });

        return Ok();
    }

    [HttpPost(ApiPaths.TeamUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeTeam(TeamUserRole.Owner)]
    public async Task<IActionResult> Update([FromRoute] string teamId, [FromRoute] string userId,
        [FromBody] UpdateTeamUser model)
    {
        await requestHandler.Handle(new UpdateTeamUserRequest
        {
            TeamId = teamId,
            UserId = userId,
            Model = model
        });
        
        return Ok();
    }

    [HttpDelete(ApiPaths.TeamUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeTeam(TeamUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string teamId, [FromRoute] string userId)
    {
        await requestHandler.Handle(new DeleteTeamUserRequest
        {
            TeamId = teamId,
            UserId = userId
        });
        
        return Ok();
    }
}