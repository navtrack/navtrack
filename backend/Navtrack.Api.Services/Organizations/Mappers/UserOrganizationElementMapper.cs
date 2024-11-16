using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class UserOrganizationElementMapper
{
    public static UserOrganizationElement Map(ObjectId organizationId, OrganizationUserRole userRole, ObjectId userId)
    {
        return new UserOrganizationElement
        {
            UserRole = userRole,
            OrganizationId = organizationId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId
        };
    }
}