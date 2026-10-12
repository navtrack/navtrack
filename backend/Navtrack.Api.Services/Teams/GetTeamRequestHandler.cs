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
    public override async Task<TeamModel> Handle(GetTeamRequest request)
    {
        TeamEntity? team = await teamRepository.GetById(request.TeamId);
        team.Return404IfNull();
        
        TeamModel result = TeamMapper.Map(team);

        return result;
    }
}
