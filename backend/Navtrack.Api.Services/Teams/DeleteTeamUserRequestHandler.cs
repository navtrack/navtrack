using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Events;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Teams;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<DeleteTeamUserRequest>))]
public class DeleteTeamUserRequestHandler(ITeamRepository teamRepository, IUserRepository userRepository)
    : BaseRequestHandler<DeleteTeamUserRequest>
{
    private TeamEntity? team;
    private UserEntity? user;
    
    public override async Task Validate(RequestValidationContext<DeleteTeamUserRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        user = await userRepository.GetById(context.Request.UserId);
        user.Return404IfNull();
        
        bool hasTeam = user.Teams.All(x => x.Id != team.Id);
        hasTeam.Return404IfTrue();
    }

    public override async Task Handle(DeleteTeamUserRequest source)
    {
        await userRepository.RemoveTeamFromUser(user!.Id, team!.Id);
        await teamRepository.UpdateUsersCount(team.Id);
    }

    public override IEvent GetEvent(DeleteTeamUserRequest request)
    {
        return new TeamUserDeletedEvent
        {
            TeamId = team!.Id.ToString(),
            UserId = user!.Id.ToString()
        };
    }
}