using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Shared;

namespace Navtrack.DataAccess.Model.Assets;

public class AssetTeamElement : CreatedAuditElement
{
    [BsonElement("teamId")]
    public ObjectId TeamId { get; set; }
}