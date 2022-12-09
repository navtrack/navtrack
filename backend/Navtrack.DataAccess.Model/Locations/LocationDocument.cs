using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Navtrack.DataAccess.Model.Attributes;

namespace Navtrack.DataAccess.Model.Locations;

[Collection("locations")]
public class LocationDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("dId")]
    public ObjectId DeviceId { get; set; }

    [BsonElement("aId")]
    public ObjectId AssetId { get; set; }

    /// <summary>
    /// [longitude, latitude]
    /// </summary>
    [BsonElement("c")]
    public double[] Coordinates { get; set; }

    [BsonIgnore]
    public double Latitude => Coordinates[1];
        
    [BsonIgnore]
    public double Longitude => Coordinates[0];

    [BsonElement("cD")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("d")]
    public DateTime DateTime { get; set; }

    [BsonElement("s")]
    public float? Speed { get; set; }

    [BsonElement("h")]
    public float? Heading { get; set; }

    [BsonElement("a")]
    public float? Altitude { get; set; }

    [BsonElement("st")]
    public int? Satellites { get; set; }

    [BsonElement("hd")]
    public float? HDOP { get; set; }

    [BsonElement("v")]
    public bool? Valid { get; set; }

    [BsonElement("g")]
    public short? GsmSignal { get; set; }

    [BsonElement("o")]
    public double? Odometer { get; set; }

    [BsonElement("cgi")]
    public CellGlobalIdentityElement CellGlobalIdentity { get; set; }
        
    [BsonElement("cId")]
    public ObjectId? DeviceConnectionMessageId { get; set; }
}