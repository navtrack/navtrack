using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet(ApiPaths.Health)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        Version? version = GetType().Assembly.GetName().Version;
        
        return Ok($"Choco and Milk says hi! :) v{version}");
    }
}