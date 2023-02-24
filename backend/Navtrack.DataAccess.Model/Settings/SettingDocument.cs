using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Attributes;

namespace Navtrack.DataAccess.Model.Settings;

[Collection("settings")]
public class SettingDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("key")]
    public string Key { get; set; }
  
    [BsonElement("value")]
    public BsonDocument Value { get; set; }
}