using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Navtrack.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok("Choco say hi.");
    }
}