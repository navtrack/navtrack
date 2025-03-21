using System.Collections.Generic;

namespace Navtrack.Api.Model.Common;

public class Error
{
    public IDictionary<string, string[]>? Errors { get; set; }
    public string Code { get; set; }
    public string? Message { get; set; }
}