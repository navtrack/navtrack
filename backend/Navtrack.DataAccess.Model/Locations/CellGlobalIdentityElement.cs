using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Locations;

public class CellGlobalIdentityElement
{
    [BsonElement("c")]
    public int? MobileCountryCode { get; set; }

    [BsonElement("n")]
    public int? MobileNetworkCode { get; set; }

    [BsonElement("a")]
    public int? LocationAreaCode { get; set; }

    [BsonElement("i")]
    public int? CellId { get; set; }
}