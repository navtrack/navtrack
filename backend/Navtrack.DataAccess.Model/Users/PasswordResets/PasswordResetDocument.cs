using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Users.PasswordResets;

[Collection("users_password_resets")]
public class PasswordResetDocument : BaseDocument
{
    [BsonElement("email")]
    public string Email { get; set; }
    
    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; }
        
    [BsonElement("createdBy")]
    public ObjectId CreatedBy { get; set; }

    [BsonElement("ipAddress")]
    public string IpAddress { get; set; }
    
    [BsonElement("hash")]
    public string Hash { get; set; }
    
    [BsonElement("invalid")]
    public bool Invalid { get; set; }
}