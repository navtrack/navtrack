using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace Navtrack.Api.Model.Custom
{
    public class ApiResponseModel
    {
        [JsonPropertyName("message")] 
        public string Message { get; set; }
        
        [JsonPropertyName("fields")]
        public Dictionary<string, string> Fields { get; set;  } 
        
        [JsonIgnore]
        public bool IsValid => Fields == null || !Fields.Any() && string.IsNullOrEmpty(Message);

        public HttpStatusCode? HttpStatusCode { get; set; }

        public void AddError(string property, string message)
        {
            property = char.ToLowerInvariant(property[0]) + property.Substring(1);
            
            if (Fields == null)
            {
                Fields = new Dictionary<string, string>();
            }

            Fields[property] = message;
        }

        public void IsUnauthorised()
        {
            HttpStatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}