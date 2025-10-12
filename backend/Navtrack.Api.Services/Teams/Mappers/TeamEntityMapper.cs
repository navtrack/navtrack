using System;
using Navtrack.Api.Model.Teams;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Services.Teams.Mappers;

public static class TeamEntityMapper
{
    public static TeamEntity Map(CreateTeam source, Guid organizationId, Guid userId)
    {
        TeamEntity destination = new()
        {
            Name = source.Name.Trim(),
            OrganizationId = organizationId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId
        };

        return destination;
    }
}