using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Protocols;
using Navtrack.Api.Services.Requests;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class ProtocolsController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.Protocols)]
    [ProducesResponseType(typeof(ListModel<ProtocolModel>), StatusCodes.Status200OK)]
    public Task<ListModel<ProtocolModel>> GetList() =>
        Query<GetProtocolsRequest, ListModel<ProtocolModel>>(new GetProtocolsRequest());
}
