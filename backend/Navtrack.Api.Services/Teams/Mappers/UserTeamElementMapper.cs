using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Teams.Mappers;

public static class UserTeamElementMapper
{
    public static UserTeamElement Map(ObjectId teamId, TeamUserRole userRole, ObjectId currentUserId, ObjectId organizationId)
    {
        return new UserTeamElement
        {
            UserRole = userRole,
            TeamId = teamId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId,
            OrganizationId = organizationId
        };
    }
}