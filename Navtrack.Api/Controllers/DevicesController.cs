using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Custom;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.IdentityServer;

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
        public Task<ActionResult<DeviceResponseModel>> Get([FromRoute] int id)
        {
            return HandleCommand<GetDeviceByIdCommand, DeviceResponseModel>(new GetDeviceByIdCommand
            {
                DeviceId = id,
                UserId = User.GetId()
            });
        }

        [HttpGet("{id}/statistics")]
        public Task<ActionResult<DeviceStatisticsResponseModel>> GetStatistics([FromRoute] int id)
        {
            return HandleCommand<GetDeviceStatisticsCommand, DeviceStatisticsResponseModel>(
                new GetDeviceStatisticsCommand
                {
                    DeviceId = id,
                    UserId = User.GetId()
                });
        }

        [HttpGet("{id}/connections")]
        public Task<ActionResult<ResultsPaginationResponseModel<DeviceConnectionResponseModel>>> GetConnections(
            [FromRoute] DeviceConnectionRequestModel model)
        {
            return HandleCommand<GetDeviceConnectionsCommand, ResultsPaginationResponseModel<DeviceConnectionResponseModel>>(
                new GetDeviceConnectionsCommand
                {
                    DeviceId = model.Id,
                    Page = model.Page,
                    PerPage = model.PerPage,
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