using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Attributes;

namespace Navtrack.DataAccess.Model.Users;

[Collection("refreshTokens")]
public class RefreshTokenDocument
{
    /// <summary>
    /// Represents the user id.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }
    public DateTime CreationTime { get; set; }
    public int Lifetime { get; set; }
    public DateTime? ConsumedTime { get; set; }
    public AccessTokenElement AccessToken { get; set; }
    public int Version { get; set; }
}