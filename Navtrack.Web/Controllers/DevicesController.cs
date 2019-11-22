using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Common.Model;
using Navtrack.Web.Model.Models;
using Navtrack.Web.Services;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            this.deviceService = deviceService;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(DeviceModel model)
        {
            await deviceService.Update(model);

            return Ok();
        }
        
        [HttpGet]
        public Task<List<DeviceModel>> Get()
        {
            return deviceService.Get();
        }
        
        [HttpGet("protocols")]
        public List<ProtocolModel> GetProtocols()
        {
            return deviceService.GetProtocols();
        }

        [HttpGet("{id}")]
        public Task<DeviceModel> Get(int id)
        {
            return deviceService.Get(id);
        }
    }
}