using System.Text.Json.Serialization;

namespace Navtrack.Api.Model.Custom
{
    public class ApiFieldError
    {
        [JsonPropertyName("name")] 
        public string Name { get; set; }
        
        [JsonPropertyName("message")] 
        public string Message { get; set; }
    }
}