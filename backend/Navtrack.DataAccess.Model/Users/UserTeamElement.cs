using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Shared;
using Navtrack.DataAccess.Model.Teams;

namespace Navtrack.DataAccess.Model.Users;

public class UserTeamElement : UpdatedAuditElement
{
    [BsonElement("teamId")]
    public ObjectId TeamId { get; set; }
    
    [BsonElement("organizationId")]
    public ObjectId OrganizationId { get; set; }
    
    [BsonElement("role")]
    [BsonRepresentation(BsonType.String)]
    public TeamUserRole UserRole { get; set; }
}