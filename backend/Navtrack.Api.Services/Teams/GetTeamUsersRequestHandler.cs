using System.Net;
using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Teams;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<GetTeamUsersRequest, List<TeamUser>>))]
public class GetTeamUsersRequestHandler(ITeamRepository teamRepository, IUserRepository userRepository)
    : BaseRequestHandler<GetTeamUsersRequest, List<TeamUser>>
{
    private TeamDocument? team;
    public override async Task Validate(RequestValidationContext<GetTeamUsersRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.ThrowApiExceptionIfNull(HttpStatusCode.NotFound);
    }

    public override async Task<List<TeamUser>> Handle(GetTeamUsersRequest request)
    {
        System.Collections.Generic.List<UserDocument> users = await userRepository.GetByTeamId(request.TeamId);

        List<TeamUser> result = ListMapper.Map(users, x => TeamUserMapper.Map(x, team!));
        
        return result;
    }
}