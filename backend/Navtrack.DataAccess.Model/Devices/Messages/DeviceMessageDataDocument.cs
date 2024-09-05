using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Model.Devices.Messages;

[Collection("devices_messages_data")]
public class DeviceMessageDataDocument : BaseDocument
{
    [BsonElement("did")]
    public ObjectId DeviceMessageId { get; set; }

    [BsonElement("ad")]
    public Dictionary<string, string>? AdditionalData { get; set; }
    
    [BsonElement("adu")]
    public Dictionary<string, string>? AdditionalDataUnhandled { get; set; }
}