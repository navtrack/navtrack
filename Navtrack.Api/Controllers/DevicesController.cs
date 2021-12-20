using System.Net.Mime;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Devices;

namespace Navtrack.Api.Controllers;

[ApiController]
[Route("devices")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class DevicesController : ControllerBase
{
    private readonly IDeviceTypeService deviceTypeService;

    public DevicesController(IDeviceTypeService deviceTypeService)
    {
        this.deviceTypeService = deviceTypeService;
    }

    [HttpGet("types")]
    [ProducesResponseType(typeof(DeviceTypeListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    public IActionResult GetTypes()
    {
        return new JsonResult(deviceTypeService.GetDeviceTypes());
    }
}