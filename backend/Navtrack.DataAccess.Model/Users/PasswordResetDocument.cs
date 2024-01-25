using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Users;

[Collection("passwordResets")]
public class PasswordResetDocument : BaseDocument
{
    [BsonElement("email")]
    public string Email { get; set; }
    
    [BsonElement("userId")]
    public ObjectId UserId { get; set; }
    
    [BsonElement("created")]
    public AuditElement Created { get; set; }
    
    [BsonElement("ipAddress")]
    public string IpAddress { get; set; }
    
    [BsonElement("hash")]
    public string Hash { get; set; }
    
    [BsonElement("invalid")]
    public bool Invalid { get; set; }
}