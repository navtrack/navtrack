using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Shared;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Organizations;

[Collection("organizations")]
public class OrganizationDocument : UpdatedAuditDocument
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("assetsCount")]
    public int AssetsCount { get; set; }
    
    [BsonElement("usersCount")]
    public int UsersCount { get; set; }
    
    [BsonElement("teamsCount")]
    public int TeamsCount { get; set; }

    [BsonElement("devicesCount")]
    public int DevicesCount { get; set; }
}