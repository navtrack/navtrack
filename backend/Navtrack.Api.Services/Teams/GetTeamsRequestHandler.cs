using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Services.Organizations;
using Navtrack.DataAccess.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<GetTeamsRequest, List<Team>>))]
public class GetTeamsRequestHandler(
    ITeamRepository teamRepository,
    IOrganizationRepository organizationRepository,
    INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<GetTeamsRequest, List<Team>>
{
    public override async Task Validate(RequestValidationContext<GetTeamsRequest> context)
    {
        OrganizationDocument? organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }

    public override async Task<List<Team>> Handle(GetTeamsRequest request)
    {
        System.Collections.Generic.List<TeamDocument> teams = await GetTeams(request.OrganizationId);

        List<Team> result = ListMapper.Map(teams, TeamMapper.Map);

        return result;
    }

    private Task<System.Collections.Generic.List<TeamDocument>> GetTeams(string organizationId)
    {
        if (navtrackContextAccessor.NavtrackContext.HasOrganizationUserRole(organizationId, OrganizationUserRole.Owner))
        {
            return teamRepository.GetByOrganizationId(ObjectId.Parse(organizationId));
        }

        System.Collections.Generic.List<ObjectId> teamIds = navtrackContextAccessor.NavtrackContext.User?.Teams?
            .Where(x => x.OrganizationId == ObjectId.Parse(organizationId))
            .Select(x => x.TeamId).ToList() ?? [];

        return teamRepository.GetByIds(teamIds);
    }
}