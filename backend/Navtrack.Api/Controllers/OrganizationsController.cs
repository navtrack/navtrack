using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Organizations;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.Organizations)]
public class OrganizationsController(INavtrackRequestContextAccessor navtrackRequestContextAccessor, IRequestHandler requestHandler)
    : BaseOrganizationsController(requestHandler, navtrackRequestContextAccessor)
{
    [HttpGet(ApiPaths.Organizations)]
    [ProducesResponseType(typeof(ListModel<OrganizationModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ListModel<OrganizationModel>> List() =>
        Query<GetOrganizationsRequest, ListModel<OrganizationModel>>(new GetOrganizationsRequest());

    [HttpGet(ApiPaths.OrganizationById)]
    [ProducesResponseType(typeof(OrganizationModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(OrganizationUserRole.Member)]
    public Task<OrganizationModel> Get([FromRoute] string organizationId) =>
        Query<GetOrganizationRequest, OrganizationModel>(new GetOrganizationRequest
        {
            OrganizationId = organizationId
        });
}
