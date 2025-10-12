using System.Net;
using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Teams;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<GetTeamUsersRequest, ListModel<TeamUser>>))]
public class GetTeamUsersRequestHandler(ITeamRepository teamRepository, IUserRepository userRepository)
    : BaseRequestHandler<GetTeamUsersRequest, ListModel<TeamUser>>
{
    private TeamEntity? team;
    public override async Task Validate(RequestValidationContext<GetTeamUsersRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.ThrowApiExceptionIfNull(HttpStatusCode.NotFound);
    }

    public override async Task<ListModel<TeamUser>> Handle(GetTeamUsersRequest request)
    {
        System.Collections.Generic.List<UserEntity> users = await userRepository.GetByTeamId(team.Id);

        ListModel<TeamUser> result = ListMapper.Map(users, x => TeamUserMapper.Map(x, team!));
        
        return result;
    }
}