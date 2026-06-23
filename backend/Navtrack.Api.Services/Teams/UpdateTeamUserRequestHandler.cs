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
    private TeamEntity? team;
    private UserEntity? user;

    public override async Task Validate(RequestValidationContext<UpdateTeamUserRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        user = await userRepository.GetById(context.Request.UserId);
        user.Return404IfNull();

        TeamEntity? userTeam = user.Teams.FirstOrDefault(x => x.Id == team.Id);
        userTeam.Return404IfNull();
    }

    public override Task Handle(UpdateTeamUserRequest source)
    {
        return userRepository.UpdateTeamUser(team!.Id, user!.Id, source.Model.UserRole);
    }
}