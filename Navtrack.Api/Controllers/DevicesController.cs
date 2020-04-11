using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Models;
using Navtrack.Api.Services;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Controllers
{
    public class DevicesController : GenericController<Device, DeviceModel>
    {
        private readonly IDeviceService deviceService;

        public DevicesController(IDeviceService deviceService) : base(deviceService)
        {
            this.deviceService = deviceService;
        }

        [HttpGet("available")]
        [HttpGet("available/{id}")]
        public Task<List<DeviceModel>> GetAll(int? id)
        {
            return deviceService.GetAllAvailableForAsset(id);
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