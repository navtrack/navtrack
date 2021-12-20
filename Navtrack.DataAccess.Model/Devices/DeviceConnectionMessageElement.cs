using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices;

public class DeviceConnectionMessageElement
{
    public DeviceConnectionMessageElement()
    {
        Id = ObjectId.GenerateNewId();
    }

    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("hex")]
    public string Hex { get; set; }
}