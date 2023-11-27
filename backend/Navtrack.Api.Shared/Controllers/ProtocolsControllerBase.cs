using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Protocols;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
public abstract class ProtocolsControllerBase : ControllerBase
{
    private readonly IProtocolService protocolService;

    protected ProtocolsControllerBase(IProtocolService protocolService)
    {
        this.protocolService = protocolService;
    }

    [HttpGet(ApiPaths.Protocols)]
    [ProducesResponseType(typeof(ListModel<ProtocolModel>), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public ListModel<ProtocolModel> GetProtocols()
    {
        return protocolService.GetProtocols();
    }
}