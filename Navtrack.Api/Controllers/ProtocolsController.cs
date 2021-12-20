using System.Net.Mime;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Old.Protocols;

namespace Navtrack.Api.Controllers;

[ApiController]
[Route("protocols")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class ProtocolsController : ControllerBase
{
    private readonly IProtocolService protocolService;

    public ProtocolsController(IProtocolService protocolService)
    {
        this.protocolService = protocolService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProtocolListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    public ProtocolListModel GetProtocols()
    {
        return protocolService.GetProtocols();
    }
}