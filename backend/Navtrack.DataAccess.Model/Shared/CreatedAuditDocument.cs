using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Shared;

public class CreatedAuditDocument : BaseDocument
{
    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("createdBy")]
    public ObjectId CreatedBy { get; set; }
}