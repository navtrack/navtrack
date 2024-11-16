using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Events;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Teams;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<CreateTeamUserRequest>))]
public class CreateTeamUserRequestHandler(
    ITeamRepository teamRepository,
    IUserRepository userRepository,
    INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<CreateTeamUserRequest>
{
    private TeamDocument? team;
    private UserDocument? user;

    public override async Task Validate(RequestValidationContext<CreateTeamUserRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        user = await userRepository.GetByEmail(context.Request.Model.Email);
        user.ThrowValidationApiIfNull(x => x.AddValidationError(nameof(context.Request.Model.Email),
            ApiErrorCodes.User_000001_EmailNotFound));

        context.ValidationException.AddErrorIfTrue(
            !user.Organizations?.Any(x => x.OrganizationId == team.OrganizationId),
            nameof(context.Request.Model.Email),
            ApiErrorCodes.Team_000006_UserNotInSameOrganization);
        
        context.ValidationException.AddErrorIfTrue(
            user.Teams?.Any(x => x.TeamId == team.Id),
            nameof(context.Request.Model.Email),
            ApiErrorCodes.Team_000007_UserAlreadyInTeam);
    }

    public override async Task Handle(CreateTeamUserRequest source)
    {
        UserTeamElement element =
            UserTeamElementMapper.Map(team!.Id, source.Model.UserRole, navtrackContextAccessor.NavtrackContext.User.Id,
                team.OrganizationId);

        await userRepository.AddUserToTeam(user!.Id, element);
        await teamRepository.UpdateUsersCount(team.Id);
    }

    public override IEvent GetEvent(CreateTeamUserRequest request)
    {
        return new TeamUserCreatedEvent
        {
            TeamId = request.TeamId,
            UserId = user!.Id.ToString()
        };
    }
}