using System.Threading.Tasks;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<GetTeamRequest, Team>))]
public class GetTeamRequestHandler(ITeamRepository teamRepository) : BaseRequestHandler<GetTeamRequest, Team>
{
    private TeamDocument? team;

    public override async Task Validate(RequestValidationContext<GetTeamRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();
    }

    public override Task<Team> Handle(GetTeamRequest request)
    {
        Team result = TeamMapper.Map(team!);

        return Task.FromResult(result);
    }
}