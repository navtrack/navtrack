using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Teams;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<UpdateTeamUserRequest>))]
public class UpdateTeamUserRequestHandler(ITeamRepository teamRepository, IUserRepository userRepository)
    : BaseRequestHandler<UpdateTeamUserRequest>
{
    public override async Task Handle(UpdateTeamUserRequest source)
    {
        TeamEntity? team = await teamRepository.GetById(source.TeamId);
        team.Return404IfNull();

        UserEntity? user = await userRepository.GetById(source.UserId);
        user.Return404IfNull();

        TeamEntity? userTeam = user.Teams.FirstOrDefault(x => x.Id == team.Id);
        userTeam.Return404IfNull();
        
        await userRepository.UpdateTeamUser(team.Id, user.Id, source.Model.UserRole);
    }
}
