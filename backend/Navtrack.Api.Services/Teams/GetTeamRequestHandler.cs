using System.Threading.Tasks;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<GetTeamRequest, TeamModel>))]
public class GetTeamRequestHandler(ITeamRepository teamRepository) : BaseRequestHandler<GetTeamRequest, TeamModel>
{
    private TeamEntity? team;

    public override async Task Validate(RequestValidationContext<GetTeamRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();
    }

    public override Task<TeamModel> Handle(GetTeamRequest request)
    {
        TeamModel result = TeamMapper.Map(team!);

        return Task.FromResult(result);
    }
}