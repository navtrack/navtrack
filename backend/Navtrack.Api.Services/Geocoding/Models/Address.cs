using Newtonsoft.Json;

namespace Navtrack.Api.Services.Geocoding.Models;

internal class Address
{
    [JsonProperty("road")]
    public string? Road { get; set; }

    [JsonProperty("suburb")]
    public string? Suburb { get; set; }

    [JsonProperty("village")]
    public string? Village { get; set; }

    [JsonProperty("town")]
    public string? Town { get; set; }

    [JsonProperty("city")]
    public string? City { get; set; }

    [JsonProperty("county")]
    public string? County { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }

    [JsonProperty("postcode")]
    public string? Postcode { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("country_code")]
    public string CountryCode { get; set; }
}