using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Attributes;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model.Users;

[Collection("users")]
public class UserDocument
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("email")]
    public string Email { get; set; }
        
    [BsonElement("units")]
    [BsonRepresentation(BsonType.String)]
    public UnitsType UnitsType { get; set; }

    [BsonElement("password")]
    public PasswordElement Password { get; set; }
        
    [BsonElement("created")]
    public AuditElement Created { get; set; }
        
    [BsonElement("assetRoles")]
    public IEnumerable<UserAssetRoleElement>? AssetRoles { get; set; }

    [BsonElement("googleId")]
    public string? GoogleId { get; set; }

    [BsonElement("microsoftId")]
    public string? MicrosoftId { get; set; }

    [BsonElement("appleId")]
    public string? AppleId { get; set; }
}