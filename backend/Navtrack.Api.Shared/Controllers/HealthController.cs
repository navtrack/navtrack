using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        Version? version = GetType().Assembly.GetName().Version;
        
        return Ok($"Choco says hi! :) v{version}");
    }
}