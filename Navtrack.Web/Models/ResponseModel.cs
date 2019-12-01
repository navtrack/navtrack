using System.Text.Json.Serialization;

namespace Navtrack.Web.Models
{
    public class ResponseModel
    {
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }
}