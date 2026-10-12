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

[Service(typeof(IRequestHandler<DeleteTeamUserRequest>))]
public class DeleteTeamUserRequestHandler(ITeamRepository teamRepository, IUserRepository userRepository)
    : BaseRequestHandler<DeleteTeamUserRequest>
{
    public override async Task Handle(DeleteTeamUserRequest source)
    {
        TeamEntity? team = await teamRepository.GetById(source.TeamId);
        team.Return404IfNull();

        UserEntity? user = await userRepository.GetById(source.UserId);
        user.Return404IfNull();
        
        bool hasTeam = user.Teams.All(x => x.Id != team.Id);
        hasTeam.Return404IfTrue();
        
        await userRepository.RemoveTeamFromUser(user.Id, team.Id);
        await teamRepository.UpdateUsersCount(team.Id);
    }
}
