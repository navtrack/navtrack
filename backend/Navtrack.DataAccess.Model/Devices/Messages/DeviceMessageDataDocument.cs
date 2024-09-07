using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Devices.Messages;

[Collection("devices_messages_data")]
public class DeviceMessageDataDocument : BaseDocument
{
    [BsonElement("dmid")]
    public ObjectId DeviceMessageId { get; set; }

    [BsonElement("d")]
    public Dictionary<string, string>? Data { get; set; }
    
    [BsonElement("du")]
    public Dictionary<string, string>? DataUnhandled { get; set; }

    [BsonElement("de")]
    public Dictionary<string, string>? DataException { get; set; }

    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }
}