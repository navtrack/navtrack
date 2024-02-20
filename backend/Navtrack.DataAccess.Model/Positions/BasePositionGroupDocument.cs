using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Positions;

public class BasePositionGroupDocument : BaseDocument
{
    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("aId")]
    public ObjectId AssetId { get; set; }

    [BsonElement("dId")]
    public ObjectId DeviceId { get; set; }

    [BsonElement("cId")]
    public ObjectId? ConnectionId { get; set; }

    [BsonElement("sd")]
    public DateTime StartDate { get; set; }
    
    [BsonElement("ed")]
    public DateTime EndDate { get; set; }
}