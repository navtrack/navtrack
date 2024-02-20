using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Positions;

[Collection("positions")]
public class PositionGroupDocument : BasePositionGroupDocument
{
    [BsonElement("p")]
    public required List<PositionElement> Positions { get; set; }
}