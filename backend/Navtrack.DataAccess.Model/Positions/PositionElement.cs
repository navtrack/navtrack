using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Positions;

public class PositionElement
{
    /// <summary>
    /// [Longitude, Latitude]
    /// </summary>
    [BsonElement("c")]
    public double[] Coordinates { get; set; }

    [BsonIgnore]
    public double Latitude => Coordinates[1];
        
    [BsonIgnore]
    public double Longitude => Coordinates[0];
    
    [BsonElement("cd")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("d")]
    public DateTime Date { get; set; }

    [BsonElement("spd")]
    public float? Speed { get; set; }

    [BsonElement("hea")]
    public float? Heading { get; set; }

    [BsonElement("alt")]
    public float? Altitude { get; set; }

    [BsonElement("sats")]
    public int? Satellites { get; set; }

    [BsonElement("hdop")]
    public float? HDOP { get; set; }

    [BsonElement("v")]
    public bool? Valid { get; set; }

    [BsonElement("gsm")]
    public short? GsmSignal { get; set; }

    [BsonElement("odo")]
    public double? Odometer { get; set; }

    [BsonElement("cgi")]
    public CellGlobalIdentityElement? CellGlobalIdentity { get; set; }
}