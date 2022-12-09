using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Common.Services.Settings;

namespace Navtrack.Api.Controllers;

[ApiController]
[Route("settings")]
public class SettingsController : ControllerBase
{
    private readonly ISettingService settingService;

    public SettingsController(ISettingService settingService)
    {
        this.settingService = settingService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get()
    {
        Dictionary<string, string> settings = await settingService.GetPublicSettings();

        return new JsonResult(settings);
    }
}