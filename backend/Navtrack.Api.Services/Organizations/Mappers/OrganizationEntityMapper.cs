using System;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class OrganizationEntityMapper
{
    public static OrganizationEntity Map(string name, Guid ownerId, OrganizationEntity? destination = null)
    {
        destination ??= new OrganizationEntity();

        destination.Name = name.Trim();
        destination.CreatedDate = DateTime.UtcNow;
        destination.CreatedBy = ownerId;

        return destination;
    }
}