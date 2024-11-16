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

[Service(typeof(IRequestHandler<UpdateTeamUserRequest>))]
public class UpdateTeamUserRequestHandler(ITeamRepository teamRepository, IUserRepository userRepository)
    : BaseRequestHandler<UpdateTeamUserRequest>
{
    private TeamDocument? team;
    private UserDocument? user;

    public override async Task Validate(RequestValidationContext<UpdateTeamUserRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        user = await userRepository.GetById(context.Request.UserId);
        user.Return404IfNull();

        UserTeamElement? userTeam = user.Teams?.FirstOrDefault(x => x.TeamId == team.Id);
        userTeam.Return404IfNull();
    }

    public override Task Handle(UpdateTeamUserRequest source)
    {
        return userRepository.UpdateTeamUser(team!.Id, user!.Id, source.Model.UserRole);
    }

    public override IEvent GetEvent(UpdateTeamUserRequest request)
    {
        return new TeamUserUpdatedEvent
        {
            TeamId = request.TeamId,
            UserId = request.UserId
        };
    }
}