using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Devices;

namespace Navtrack.Api.Controllers
{
    public class DevicesController : BaseController
    {
        private readonly IDeviceTypeService deviceTypeService;

        public DevicesController(IDeviceTypeService deviceTypeService, IServiceProvider serviceProvider) : base(
            serviceProvider)
        {
            this.deviceTypeService = deviceTypeService;
        }

        [HttpGet("types")]
        public List<DeviceTypeResponseModel> GetTypes()
        {
            return deviceTypeService.GetDeviceTypes();
        }
    }
}