using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Shared;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Teams;

[Collection("teams")]
public class TeamDocument : UpdatedAuditDocument
{
    [BsonElement("organizationId")]
    public ObjectId OrganizationId { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("assetsCount")]
    public int AssetsCount { get; set; }
    
    [BsonElement("usersCount")]
    public int UsersCount { get; set; }
}