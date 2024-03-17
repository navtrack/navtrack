using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Devices;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
public abstract class DevicesControllerBase(IDeviceTypeService typeService) : ControllerBase
{
    [HttpGet(ApiPaths.DevicesTypes)]
    [ProducesResponseType(typeof(ListModel<DeviceTypeModel>), StatusCodes.Status200OK)]
    public IActionResult GetList()
    {
        ListModel<DeviceTypeModel> model = typeService.GetAll();

        return new JsonResult(model);
    }
}