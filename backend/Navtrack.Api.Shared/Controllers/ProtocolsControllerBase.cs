using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Protocols;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
public abstract class ProtocolsControllerBase(IProtocolService service) : ControllerBase
{
    [HttpGet(ApiPaths.Protocols)]
    [ProducesResponseType(typeof(ListModel<ProtocolModel>), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public ListModel<ProtocolModel> GetList()
    {
        return service.GetProtocols();
    }
}