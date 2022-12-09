using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Users;

public class AccessTokenElement
{
    /// <summary>
    /// Represents the user id.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }
    public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    public IEnumerable<string> AllowedSigningAlgorithms { get; set; }
    public string Confirmation { get; set; }
    public IEnumerable<string> Audiences { get; set; }
    public string Issuer { get; set; }
    public DateTime CreationTime { get; set; }
    public int Lifetime { get; set; }
    public string Type { get; set; }
    public string ClientId { get; set; }
    public string AccessTokenType { get; set; }
    public string Description { get; set; }
    public int Version { get; set; }
}