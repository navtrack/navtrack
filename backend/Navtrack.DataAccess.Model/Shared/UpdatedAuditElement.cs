using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Shared;

public class UpdatedAuditElement : CreatedAuditElement
{
    [BsonElement("updatedDate")]
    public DateTime UpdatedDate { get; set; }

    [BsonElement("updatedBy")]
    public ObjectId UpdatedBy { get; set; }
}