using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Users.RefreshTokens;

[Collection("users_refresh_tokens")]
public class RefreshTokenDocument
{
    [BsonId]
    public ObjectId Id { get; set; }
    public DateTime CreationTime { get; set; }
    public int Lifetime { get; set; }
    public DateTime? ConsumedTime { get; set; }
    public AccessTokenElement AccessToken { get; set; }
    public int Version { get; set; }
    public string Hash { get; set; }
    public string JwtId { get; set; }
    public DateTime ExpiryTime { get; set; }
}