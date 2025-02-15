using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Organizations;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.Organizations)]
public class OrganizationsController(INavtrackContextAccessor navtrackContextAccessor, IRequestHandler requestHandler)
    : BaseOrganizationsController(requestHandler, navtrackContextAccessor)
{
    [HttpGet(ApiPaths.Organizations)]
    [ProducesResponseType(typeof(List<Organization>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<Organization>> List()
    {
        List<Organization> result =
            await requestHandler.Handle<GetOrganizationsRequest, List<Organization>>(
                new GetOrganizationsRequest()
            );

        return result;
    }

    [HttpGet(ApiPaths.OrganizationById)]
    [ProducesResponseType(typeof(Organization), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeOrganization(OrganizationUserRole.Member)]
    public async Task<Organization> Get([FromRoute] string organizationId)
    {
        Organization result = await requestHandler.Handle<GetOrganizationRequest, Organization>(
            new GetOrganizationRequest
            {
                OrganizationId = organizationId
            }
        );

        return result;
    }
}