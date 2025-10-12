using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Events;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Teams;
using Navtrack.Database.Services.Users;
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
    private TeamEntity? team;
    private UserEntity? user;

    public override async Task Validate(RequestValidationContext<CreateTeamUserRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        user = await userRepository.GetByEmail(context.Request.Model.Email);
        user.ThrowValidationApiIfNull(x => x.AddValidationError(nameof(context.Request.Model.Email),
            ApiErrorCodes.User_000001_EmailNotFound));

        context.ValidationException.AddErrorIfTrue(
            user.OrganizationUsers.All(x => x.OrganizationId != team.OrganizationId),
            nameof(context.Request.Model.Email),
            ApiErrorCodes.Team_000006_UserNotInSameOrganization);

        context.ValidationException.AddErrorIfTrue(
            user.Teams.Any(x => x.Id == team.Id),
            nameof(context.Request.Model.Email),
            ApiErrorCodes.Team_000007_UserAlreadyInTeam);
    }

    public override async Task Handle(CreateTeamUserRequest source)
    {
        TeamUserEntity entity = TeamUserEntityMapper.Map(team!.Id, user!.Id, source.Model.UserRole,
            navtrackContextAccessor.NavtrackContext.User.Id);

        await userRepository.AddUserToTeam(entity);
        await teamRepository.UpdateUsersCount(team.Id);
    }

    public override IEvent GetEvent(CreateTeamUserRequest request)
    {
        return new TeamUserCreatedEvent
        {
            TeamId = team!.Id.ToString(),
            UserId = user!.Id.ToString()
        };
    }
}