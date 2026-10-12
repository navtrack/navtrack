using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Teams;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<CreateTeamUserRequest>))]
public class CreateTeamUserRequestHandler(
    ITeamRepository teamRepository,
    IUserRepository userRepository,
    INavtrackRequestContextAccessor navtrackRequestContextAccessor)
    : BaseRequestHandler<CreateTeamUserRequest>
{
    public override async Task Handle(CreateTeamUserRequest source)
    {
        TeamEntity? team = await teamRepository.GetById(source.TeamId);
        team.Return404IfNull();

        UserEntity? user = await userRepository.GetByEmail(source.Model.Email);
        user.ThrowValidationApiIfNull(x => x.AddValidationError(nameof(source.Model.Email),
            ApiErrorCodes.User_EmailNotFound));

        ValidationApiException validationException = new();
        validationException.AddErrorIfTrue(
            user.OrganizationUsers.All(x => x.OrganizationId != team.OrganizationId),
            nameof(source.Model.Email),
            ApiErrorCodes.Team_UserNotInSameOrganization);

        validationException.AddErrorIfTrue(
            user.Teams.Any(x => x.Id == team.Id),
            nameof(source.Model.Email),
            ApiErrorCodes.Team_UserAlreadyInTeam);
        validationException.ThrowIfInvalid();
        
        TeamUserEntity entity = TeamUserEntityMapper.Map(team.Id, user.Id, source.Model.UserRole,
            navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id);

        await userRepository.AddUserToTeam(entity);
        await teamRepository.UpdateUsersCount(team.Id);
    }
}
