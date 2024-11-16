using System.Linq;
using Navtrack.Api.Model.Teams;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class TeamUserMapper
{
    public static TeamUser Map(UserDocument user, TeamDocument team)
    {
        UserTeamElement userTeam = user.Teams!.First(x => x.TeamId == team.Id);
        
        return new TeamUser
        {
            Email = user.Email,
            UserId = user.Id.ToString(),
            UserRole = userTeam.UserRole,
            CreatedDate = userTeam.CreatedDate
        };
    }
}