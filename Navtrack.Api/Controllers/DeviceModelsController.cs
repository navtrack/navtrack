using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Models;
using Navtrack.Api.Services;

namespace Navtrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesModelsController : ControllerBase
    {
        private readonly IDeviceModelService deviceModelService;

        public DevicesModelsController(IDeviceModelService deviceModelService)
        {
            this.deviceModelService = deviceModelService;
        }

        [HttpGet]
        public List<DeviceModelModel> Get()
        {
            return deviceModelService.GetModels();
        }
    }
}