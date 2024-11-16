using System;
using MongoDB.Bson;
using Navtrack.Api.Model.Teams;
using Navtrack.DataAccess.Model.Teams;

namespace Navtrack.Api.Services.Teams.Mappers;

public static class TeamDocumentMapper
{
    public static TeamDocument Map(CreateTeam source, string organizationId, ObjectId userId)
    {
        TeamDocument destination = new()
        {
            Name = source.Name.Trim(),
            OrganizationId = ObjectId.Parse(organizationId),
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId
        };

        return destination;
    }
}