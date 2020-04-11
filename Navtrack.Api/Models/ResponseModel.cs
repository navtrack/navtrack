using System.Text.Json.Serialization;

namespace Navtrack.Api.Models
{
    public class ResponseModel
    {
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }
}