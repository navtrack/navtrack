using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace Navtrack.Api.Model
{
    public class ApiResponseModel
    {
        [JsonPropertyName("title")] 
        public string Title { get; set; }
        
        [JsonPropertyName("errors")]
        public IDictionary<string, string[]> Errors { get; set;  } 
        
        [JsonPropertyName("messages")]
        public IDictionary<string, string[]> Messages { get; set; }
        
        [JsonIgnore]
        public bool IsValid => Errors == null || !Errors.Any() && string.IsNullOrEmpty(Title);

        public HttpStatusCode? HttpStatusCode { get; set; }

        public void AddError(string property, string message)
        {
            property = char.ToLowerInvariant(property[0]) + property.Substring(1);
            
            if (Errors == null)
            {
                Errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
            }
            
            if (!Errors.ContainsKey(property))
            {
                Errors[property] = new string[] { };
            }

            Errors[property] = Errors[property].Append(message).ToArray();
        }

        public void IsUnauthorised()
        {
            HttpStatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}