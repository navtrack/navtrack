using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class CellGlobalIdentityElement
{
    [BsonElement("mcc")]
    public string? MobileCountryCode { get; set; }

    [BsonElement("mnc")]
    public string? MobileNetworkCode { get; set; }

    [BsonElement("lac")]
    public string? LocationAreaCode { get; set; }

    [BsonElement("ci")]
    public int? CellId { get; set; }

    [BsonElement("lci")]
    public uint? LteCellId { get; set; }
}