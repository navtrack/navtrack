using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class PositionElement
{
    public PositionElement()
    {
        Coordinates = new double[2];
    }
    
    /// <summary>
    /// [Longitude, Latitude]
    /// </summary>
    [BsonElement("c")]
    public double[] Coordinates { get; set; }

    [BsonIgnore]
    public double Latitude
    {
        set => Coordinates[1] = value;
        get => Coordinates[1];
    }

    [BsonIgnore]
    public double Longitude
    {
        set => Coordinates[0] = value;
        get => Coordinates[0];
    }

    [BsonElement("d")]
    public DateTime Date { get; set; }

    [BsonElement("spd")]
    public double? Speed { get; set; }

    [BsonElement("hea")]
    public double? Heading { get; set; }

    [BsonElement("alt")]
    public double? Altitude { get; set; }

    [BsonElement("sats")]
    public int? Satellites { get; set; }
    
    [BsonElement("pdop")]
    public double? PDOP { get; set; }
    
    [BsonElement("hdop")]
    public double? HDOP { get; set; }

    [BsonElement("v")]
    public bool? Valid { get; set; }
}