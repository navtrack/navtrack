using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Shared;

namespace Navtrack.DataAccess.Model.Users;

public class UserOrganizationElement : UpdatedAuditElement
{
    [BsonElement("organizationId")]
    public ObjectId OrganizationId { get; set; }

    [BsonElement("role")]
    [BsonRepresentation(BsonType.String)]
    public OrganizationUserRole UserRole { get; set; }
}