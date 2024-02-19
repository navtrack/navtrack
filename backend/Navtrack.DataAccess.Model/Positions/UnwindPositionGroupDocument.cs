using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Positions;

public class UnwindPositionGroupDocument
{
    [BsonElement("p")]
    public PositionElement Position { get; set; }
}