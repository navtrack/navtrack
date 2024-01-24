using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices;

public class DeviceConnectionMessageElement
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [BsonElement("hex")]
    public string? Hex { get; set; }
}