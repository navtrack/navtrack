using MongoDB.Bson.Serialization.Attributes;

namespace Navtrack.DataAccess.Model.Locations;

public class CoordinatesElement
{
    [BsonElement("t")]
    public double Latitude { get; set; }

    [BsonElement("n")]
    public double Longitude { get; set; }
}