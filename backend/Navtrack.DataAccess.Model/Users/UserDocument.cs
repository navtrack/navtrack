using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Shared;
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
    public PasswordElement? Password { get; set; }
        
    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; }
        
    [BsonElement("assets")]
    public IEnumerable<UserAssetElement>? Assets { get; set; }
    
    [BsonElement("teams")]
    public IEnumerable<UserTeamElement>? Teams { get; set; }
    
    [BsonElement("organizations")]
    public IEnumerable<UserOrganizationElement>? Organizations { get; set; }
}