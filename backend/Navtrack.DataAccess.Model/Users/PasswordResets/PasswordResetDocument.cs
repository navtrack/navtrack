using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Shared;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Users.PasswordResets;

[Collection("users_password_resets")]
public class PasswordResetDocument : CreatedAuditDocument
{
    [BsonElement("email")]
    public string Email { get; set; }
    
    [BsonElement("ipAddress")]
    public string IpAddress { get; set; }
    
    [BsonElement("hash")]
    public string Hash { get; set; }
    
    [BsonElement("invalid")]
    public bool Invalid { get; set; }
}