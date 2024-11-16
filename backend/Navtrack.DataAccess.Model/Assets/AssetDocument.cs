using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Model.Shared;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Assets;

[Collection("assets")]
public class AssetDocument : UpdatedAuditDocument
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("ownerId")]
    public ObjectId OwnerId { get; set; }
    
    [BsonElement("organizationId")]
    public ObjectId OrganizationId { get; set; }
    
    [BsonElement("device")]
    public AssetDeviceElement? Device { get; set; }

    [BsonElement("lastMessage")]
    public DeviceMessageDocument? LastMessage { get; set; }

    [BsonElement("lastPositionMessage")]
    public DeviceMessageDocument? LastPositionMessage { get; set; }

    [BsonElement("teams")]
    public IEnumerable<AssetTeamElement>? Teams { get; set; }
}