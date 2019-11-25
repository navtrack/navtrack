using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
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

        [HttpGet("{id}")]
        public Task<DeviceModel> Get(int id)
        {
            return deviceService.Get(id);
        }
        
        [HttpGet]
        public Task<List<DeviceModel>> GetAll()
        {
            return deviceService.GetAll();
        }
        
        [HttpGet("available")]
        [HttpGet("available/{id}")]
        public Task<List<DeviceModel>> GetAll(int? id)
        {
            return deviceService.GetAllAvailableIncluding(id);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DeviceModel device)
        {
            await deviceService.ValidateModel(device, ModelState);
            
            if (ModelState.IsValid)
            {
                await deviceService.Add(device);

                return Ok();
            }
            
            return ValidationProblem();
        }
        
        [HttpPut]
        public async Task<IActionResult> Update(DeviceModel device)
        {
            await deviceService.ValidateModel(device, ModelState);
            
            if (ModelState.IsValid)
            {
                await deviceService.Update(device);

                return Ok();
            }

            return ValidationProblem();
        }

        [HttpGet("protocols")]
        public IEnumerable<ProtocolModel> GetProtocols()
        {
            return deviceService.GetProtocols();
        }
        
        [HttpGet("types")]
        public Task<List<DeviceTypeModel>> GetTypes()
        {
            return deviceService.GetTypes();
        }
    }
}