using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Users;

public class PasswordElement
{
    [BsonElement("hash")]
    public string Hash { get; set; }

    [BsonElement("salt")]
    public string Salt { get; set; }
}