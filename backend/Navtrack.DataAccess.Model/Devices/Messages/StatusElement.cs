using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class StatusElement
{
    [BsonElement("m")]
    public bool? Movement { get; set; }
    //
    // /// <summary>
    // /// 0 - No Sleep
    // /// 1 – GPS Sleep
    // /// 2 – Deep Sleep
    // /// 3 – Online Sleep
    // /// 4 - Ultra Sleep
    // /// </summary>
    // public byte? SleepMode { get; set; }
}