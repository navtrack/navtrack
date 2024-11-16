using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Events;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Teams;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<DeleteTeamUserRequest>))]
public class DeleteTeamUserRequestHandler(ITeamRepository teamRepository, IUserRepository userRepository)
    : BaseRequestHandler<DeleteTeamUserRequest>
{
    private TeamDocument? team;
    private UserDocument? user;
    
    public override async Task Validate(RequestValidationContext<DeleteTeamUserRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        user = await userRepository.GetById(context.Request.UserId);
        user.Return404IfNull();
        
        UserTeamElement? userTeam = user.Teams?.FirstOrDefault(x => x.TeamId == team.Id);
        userTeam.Return404IfNull();
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
            TeamId = request.TeamId,
            UserId = request.UserId
        };
    }
}