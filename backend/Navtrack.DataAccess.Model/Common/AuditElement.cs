using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Common;

public class AuditElement
{
    [BsonElement("date")]
    public DateTime Date { get; set; }
        
    [BsonElement("userId")]
    public ObjectId? UserId { get; set; }
}