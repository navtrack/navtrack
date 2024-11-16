using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Organizations;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class OrganizationDocumentMapper
{
    public static OrganizationDocument Map(string name, ObjectId ownerId, OrganizationDocument? destination = null)
    {
        destination ??= new OrganizationDocument();

        destination.Name = name.Trim();
        destination.CreatedDate = DateTime.UtcNow;
        destination.CreatedBy = ownerId;

        return destination;
    }
}