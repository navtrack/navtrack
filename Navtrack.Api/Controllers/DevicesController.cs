using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Devices.Requests;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.Extensions;

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

        [HttpGet("{id}")]
        public Task<ActionResult<DeviceModel>> Get([FromRoute] int id)
        {
            return HandleRequest<GetDeviceByIdRequest, DeviceModel>(new GetDeviceByIdRequest
            {
                DeviceId = id,
                UserId = User.GetId()
            });
        }

        [HttpGet("{id}/connections")]
        public Task<ActionResult<IEnumerable<DeviceConnectionResponseModel>>> GetConnections([FromRoute] int id)
        {
            return HandleRequest<GetDeviceConnectionsRequest, IEnumerable<DeviceConnectionResponseModel>>(
                new GetDeviceConnectionsRequest
                {
                    DeviceId = id,
                    UserId = User.GetId()
                });
        }

        [HttpGet("types")]
        public List<DeviceTypeResponseModel> GetTypes()
        {
            return deviceTypeService.GetDeviceTypes();
        }
    }
}