using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Models;
using Navtrack.Api.Services;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Controllers
{
    public class DevicesController : GenericController<DeviceEntity, DeviceModel>
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
    }
}