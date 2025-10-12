using System;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class OrganizationUserEntityMapper
{
    public static OrganizationUserEntity Map(Guid organizationId, Guid ownerId, OrganizationUserRole userRole, Guid createdByUserId)
    {
        return new OrganizationUserEntity
        {
            OrganizationId = organizationId,
            UserId = ownerId,
            UserRole = userRole,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = createdByUserId
        };
    }
}