using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Protocols;
using Navtrack.Api.Services.Requests;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class ProtocolsController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.Protocols)]
    [ProducesResponseType(typeof(List<Protocol>), StatusCodes.Status200OK)]
    public async Task<List<Protocol>> GetList()
    {
        List<Protocol> result = await requestHandler.Handle<GetProtocolsRequest, List<Protocol>>(
            new GetProtocolsRequest()
        );

        return result;
    }
}