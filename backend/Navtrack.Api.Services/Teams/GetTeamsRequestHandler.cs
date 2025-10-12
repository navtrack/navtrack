using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<GetTeamsRequest, ListModel<Team>>))]
public class GetTeamsRequestHandler(
    ITeamRepository teamRepository,
    IOrganizationRepository organizationRepository,
    INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<GetTeamsRequest, ListModel<Team>>
{
    private OrganizationEntity? organization;
    
    public override async Task Validate(RequestValidationContext<GetTeamsRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }

    public override async Task<ListModel<Team>> Handle(GetTeamsRequest request)
    {
        List<TeamEntity> teams = await GetTeams(organization!.Id);

        ListModel<Team> result = ListMapper.Map(teams, TeamMapper.Map);

        return result;
    }

    private Task<List<TeamEntity>> GetTeams(Guid organizationId)
    {
        if (navtrackContextAccessor.NavtrackContext.HasOrganizationUserRole(organizationId, OrganizationUserRole.Owner))
        {
            return teamRepository.GetByOrganizationId(organizationId);
        }

        List<Guid> teamIds = navtrackContextAccessor.NavtrackContext.User?.Teams
            .Where(x => x.OrganizationId == organizationId)
            .Select(x => x.Id).ToList() ?? [];

        return teamRepository.GetByIds(teamIds);
    }
}