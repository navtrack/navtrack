using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        
        [HttpPost]
        public async Task<IActionResult> Add(DeviceModel model)
        {
            if (await deviceService.IMEIAlreadyExists(model.IMEI))
            {
                ModelState.AddModelError(nameof(DeviceModel.IMEI), "IMEI already exists in the database.");

                return ValidationProblem();
            }
            else
            {
                await deviceService.Add(model);

                return Ok();
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(DeviceModel model)
        {
            await deviceService.Update(model);

            return Ok();
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