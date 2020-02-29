using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Models
{
    public class ErrorModel : ResponseModel
    {
        public ErrorModel(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                Errors = validationResult.Errors;
                Title = validationResult.Title;
            }
        }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("errors")]
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(StringComparer.Ordinal);
    }
}