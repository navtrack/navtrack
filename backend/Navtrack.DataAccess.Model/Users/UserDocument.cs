using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Users;

[Collection("users")]
public class UserDocument : BaseDocument
{
    [BsonElement("email")]
    public string Email { get; set; }
        
    [BsonElement("units")]
    [BsonRepresentation(BsonType.String)]
    public UnitsType UnitsType { get; set; }

    [BsonElement("password")]
    public PasswordElement Password { get; set; }
        
    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; }
        
    [BsonElement("assetRoles")]
    public IEnumerable<UserAssetRoleElement>? AssetRoles { get; set; }

    [BsonElement("googleId")]
    public string? GoogleId { get; set; }

    [BsonElement("microsoftId")]
    public string? MicrosoftId { get; set; }
    
    [BsonElement("appleId")]
    public string? AppleId { get; set; }
}