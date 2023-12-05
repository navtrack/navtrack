using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace Navtrack.Api.Shared;

public class BaseProgramOptions
{
    public List<Type>? Filters { get; set; }
    public Action<WebApplicationBuilder>? ConfigureServices { get; set; }
}