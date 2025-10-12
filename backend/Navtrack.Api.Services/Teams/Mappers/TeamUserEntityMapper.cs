using System;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Services.Teams.Mappers;

public static class TeamUserEntityMapper
{
    public static TeamUserEntity Map(Guid teamId, Guid userId, TeamUserRole userRole, Guid currentUserId)
    {
        return new TeamUserEntity
        {
            UserId = userId,
            UserRole = userRole,
            TeamId = teamId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId
        };
    }
}