using System.Linq;
using Navtrack.Api.Model.Teams;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class TeamUserMapper
{
    public static TeamUser Map(UserEntity user, TeamEntity team)
    {
        TeamUserEntity userTeam = user.TeamUsers.First(x => x.TeamId == team.Id);
        
        return new TeamUser
        {
            Email = user.Email,
            UserId = user.Id.ToString(),
            UserRole = userTeam.UserRole,
            CreatedDate = userTeam.CreatedDate
        };
    }
}