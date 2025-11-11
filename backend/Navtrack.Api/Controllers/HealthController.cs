using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Navtrack.Api.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet(ApiPaths.Health)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        Version? version = GetType().Assembly.GetName().Version;
        Version dotnetVersion = Environment.Version;
        
        return Ok($"Choco and Milk says hi from version {version} running .NET {dotnetVersion}. :)");
    }
}