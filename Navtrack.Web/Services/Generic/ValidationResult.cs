using System;
using System.Collections.Generic;
using System.Linq;

namespace Navtrack.Web.Services.Generic
{
    public class ValidationResult
    {
        public string Title { get; set; }
        
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(StringComparer.Ordinal);
        
        public bool IsValid => !Errors.Any() && string.IsNullOrEmpty(Title);

        public void AddError(string property, string message)
        {
            if (!Errors.ContainsKey(property))
            {
                Errors[property] = new string[] { };
            }

            Errors[property] = Errors[property].Append(message).ToArray();
        }
    }
}