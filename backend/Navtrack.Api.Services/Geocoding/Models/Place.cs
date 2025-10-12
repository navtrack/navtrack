using System.Collections.Generic;
using Newtonsoft.Json;

namespace Navtrack.Api.Services.Geocoding.Models;

internal class Place
{
    [JsonProperty("place_id")]
    public long PlaceId { get; set; }

    [JsonProperty("licence")]
    public string Licence { get; set; }

    [JsonProperty("osm_type")]
    public string OsmType { get; set; }

    [JsonProperty("osm_id")]
    public long OsmId { get; set; }

    [JsonProperty("lat")]
    public string Lat { get; set; }

    [JsonProperty("lon")]
    public string Lon { get; set; }

    [JsonProperty("place_rank")]
    public int PlaceRank { get; set; }

    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("importance")]
    public double Importance { get; set; }

    [JsonProperty("addresstype")]
    public string Addresstype { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("display_name")]
    public string DisplayName { get; set; }

    [JsonProperty("address")]
    public Address? Address { get; set; }

    [JsonProperty("boundingbox")]
    public List<double> Boundingbox { get; set; }
}