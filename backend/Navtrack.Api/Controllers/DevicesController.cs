using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.Requests;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class DevicesController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.DeviceTypes)]
    [ProducesResponseType(typeof(ListModel<DeviceTypeModel>), StatusCodes.Status200OK)]
    public async Task<ListModel<DeviceTypeModel>> GetList()
    {
        ListModel<DeviceTypeModel> result =
            await requestHandler.Handle<GetDeviceTypesRequest, ListModel<DeviceTypeModel>>(new GetDeviceTypesRequest());

        return result;
    }
}