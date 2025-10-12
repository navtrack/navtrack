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
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.Teams)]
public class TeamsController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.OrganizationTeams)]
    [ProducesResponseType(typeof(ListModel<Team>), StatusCodes.Status200OK)]
    [AuthorizeOrganization(OrganizationUserRole.Member)]
    public async Task<ListModel<Team>> GetList([FromRoute] string organizationId)
    {
        ListModel<Team> result = await requestHandler.Handle<GetTeamsRequest, ListModel<Team>>(new GetTeamsRequest
        {
            OrganizationId = organizationId
        });
            
        return result;
    }

    [HttpPost(ApiPaths.OrganizationTeams)]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string organizationId, [FromBody] CreateTeam model)
    {
        Team result = await requestHandler.Handle<CreateTeamRequest,Team>(new CreateTeamRequest
        {
            OrganizationId = organizationId,
            Model = model
        });
  
        return Ok(result);
    }
    
    [HttpGet(ApiPaths.TeamById)]
    [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
    [AuthorizeTeam(TeamUserRole.Member)]
    public async Task<Team> Get([FromRoute] string teamId)
    {
        Team result = await requestHandler.Handle<GetTeamRequest, Team>(new GetTeamRequest
        {
            TeamId = teamId
        });

        return result;
    }

    [HttpPost(ApiPaths.TeamById)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Update([FromRoute] string teamId, [FromBody] UpdateTeam model)
    {
        await requestHandler.Handle(new UpdateTeamRequest
        {
            TeamId = teamId,
            Model = model
        });

        return Ok();
    }
    
    [HttpDelete(ApiPaths.TeamById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string teamId)
    {
        await requestHandler.Handle(new DeleteTeamRequest
        {
            TeamId = teamId
        });

        return Ok();
    }
}