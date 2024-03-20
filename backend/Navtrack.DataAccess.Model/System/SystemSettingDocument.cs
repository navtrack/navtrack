using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.System;

[Collection("system_settings")]
public class SystemSettingDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("key")]
    public string Key { get; set; }
  
    [BsonElement("value")]
    public BsonDocument Value { get; set; }
}