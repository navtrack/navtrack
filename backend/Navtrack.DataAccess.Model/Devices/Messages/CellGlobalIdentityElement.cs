using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class CellGlobalIdentityElement
{
    [BsonElement("mcc")]
    public int? MobileCountryCode { get; set; }

    [BsonElement("mnc")]
    public int? MobileNetworkCode { get; set; }

    [BsonElement("lac")]
    public int? LocationAreaCode { get; set; }

    [BsonElement("ci")]
    public int? CellId { get; set; }
}