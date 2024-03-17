using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Assets;

[Collection("assets")]
public class AssetDocument : BaseDocument
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("created")]
    public AuditElement Created { get; set; }

    [BsonElement("ownerId")]
    public ObjectId OwnerId { get; set; }
    
    [BsonElement("device")]
    public AssetDeviceElement? Device { get; set; }

    [BsonElement("lastPositionMessage")]
    public MessageDocument? LastPositionMessage { get; set; }

    [BsonElement("userRoles")]
    public IEnumerable<AssetUserRoleElement> UserRoles { get; set; }
}