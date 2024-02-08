using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Assets;

[Collection("assets")]
[BsonIgnoreExtraElements]
public class AssetDocument : BaseDocument
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("created")]
    public AuditElement Created { get; set; }

    [BsonElement("device")]
    public AssetDeviceElement? Device { get; set; }

    [BsonElement("position")]
    public PositionElement? Position { get; set; }

    [BsonElement("userRoles")]
    public IEnumerable<AssetUserRoleElement> UserRoles { get; set; }
}