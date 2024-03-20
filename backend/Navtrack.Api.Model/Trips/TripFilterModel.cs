using System.Text.Json.Serialization;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Model.Trips;

public class TripFilterModel : DateFilter
{
    [JsonPropertyName("minAvgAltitude")]
    public int? MinAvgAltitude { get; set; }

    [JsonPropertyName("maxAvgAltitude")]
    public int? MaxAvgAltitude { get; set; }

    [JsonPropertyName("minAvgSpeed")]
    public int? MinAvgSpeed { get; set; }

    [JsonPropertyName("maxAvgSpeed")]
    public int? MaxAvgSpeed { get; set; }

    [JsonPropertyName("minDuration")]
    public int? MinDuration { get; set; }

    [JsonPropertyName("maxDuration")]
    public int? MaxDuration { get; set; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    [JsonPropertyName("radius")]
    public int? Radius { get; set; }
}