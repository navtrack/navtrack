using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Protocols;

namespace Navtrack.Api.Controllers;

[ApiController]
[Route("protocols")]
public class ProtocolsController : ControllerBase
{
    private readonly IProtocolService protocolService;

    public ProtocolsController(IProtocolService protocolService)
    {
        this.protocolService = protocolService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProtocolsModel), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public ProtocolsModel GetProtocols()
    {
        return protocolService.GetProtocols();
    }
}