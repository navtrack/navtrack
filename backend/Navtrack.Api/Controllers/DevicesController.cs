using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Devices;

namespace Navtrack.Api.Controllers;

[ApiController]
[Route("devices")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceTypeService deviceTypeService;

    public DevicesController(IDeviceTypeService deviceTypeService)
    {
        this.deviceTypeService = deviceTypeService;
    }

    [HttpGet("types")]
    [ProducesResponseType(typeof(DeviceTypesModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    public IActionResult GetTypes()
    {
        DeviceTypesModel model = deviceTypeService.GetAll();

        return new JsonResult(model);
    }
}