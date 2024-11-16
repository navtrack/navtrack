using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Shared;

public class CreatedAuditElement
{
    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("createdBy")]
    public ObjectId CreatedBy { get; set; }
}