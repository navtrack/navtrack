using System;
using System.Text.Json.Serialization;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Api.Model.Locations;

public class LocationFilterModel : LocationFilter
{
    [JsonPropertyName("startDate")]
    public override DateTime? StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public override DateTime? EndDate { get; set; }

    [JsonPropertyName("minAltitude")]
    public override int? MinAltitude { get; set; }

    [JsonPropertyName("maxAltitude")]
    public override int? MaxAltitude { get; set; }

    [JsonPropertyName("minSpeed")]
    public override int? MinSpeed { get; set; }

    [JsonPropertyName("maxSpeed")]
    public override int? MaxSpeed { get; set; }

    [JsonPropertyName("latitude")]
    public override double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public override double? Longitude { get; set; }

    [JsonPropertyName("radius")]
    public override int? Radius { get; set; }
}