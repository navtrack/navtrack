using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Assets;

[Collection("assets")]
public class AssetDocument : BaseDocument
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; }
        
    [BsonElement("createdBy")]
    public ObjectId CreatedBy { get; set; }

    [BsonElement("ownerId")]
    public ObjectId OwnerId { get; set; }
    
    [BsonElement("device")]
    public AssetDeviceElement? Device { get; set; }

    [BsonElement("lastMessage")]
    public DeviceMessageDocument? LastMessage { get; set; }

    [BsonElement("lastPositionMessage")]
    public DeviceMessageDocument? LastPositionMessage { get; set; }

    [BsonElement("userRoles")]
    public IEnumerable<AssetUserRoleElement> UserRoles { get; set; }
}