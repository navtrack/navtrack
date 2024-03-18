using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class GsmElement
{
    /// GSM standard TS 27.007, section 8.5 values
    /// 0        -113 dBm or less - low signal
    /// 1        -111 dBm  
    /// 2...30   -109 ... -53 dBm  
    /// 31       -51 dBm or greater - high signal
    /// 99       not known or not detectable
    [BsonElement("rssi")]
    public short? Signal { get; set; }

    [BsonElement("cgi")]
    public CellGlobalIdentityElement? CellGlobalIdentity { get; set; }
}