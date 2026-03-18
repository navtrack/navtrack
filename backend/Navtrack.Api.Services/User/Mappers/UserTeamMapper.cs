using Navtrack.Api.Model.User;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserTeamMapper
{
    public static UserTeamModel Map(TeamUserEntity source)
    {
        return new UserTeamModel
        {
            TeamId = source.TeamId.ToString(),
            UserRole = source.UserRole
        };
    }
}