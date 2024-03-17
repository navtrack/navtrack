using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Mongo;

public class BaseDocument
{
    [BsonId]
    public ObjectId Id { get; set; }
}