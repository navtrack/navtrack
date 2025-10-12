using System.Threading.Tasks;
using IdentityServer4;
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
using Navtrack.Database.Model.Organizations;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.Organizations)]
public class OrganizationsController(INavtrackContextAccessor navtrackContextAccessor, IRequestHandler requestHandler)
    : BaseOrganizationsController(requestHandler, navtrackContextAccessor)
{
    [HttpGet(ApiPaths.Organizations)]
    [ProducesResponseType(typeof(ListModel<OrganizationModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ListModel<OrganizationModel>> List()
    {
        ListModel<OrganizationModel> result =
            await requestHandler.Handle<GetOrganizationsRequest, ListModel<OrganizationModel>>(
                new GetOrganizationsRequest()
            );

        return result;
    }

    [HttpGet(ApiPaths.OrganizationById)]
    [ProducesResponseType(typeof(OrganizationModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeOrganization(OrganizationUserRole.Member)]
    public async Task<OrganizationModel> Get([FromRoute] string organizationId)
    {
        OrganizationModel result = await requestHandler.Handle<GetOrganizationRequest, OrganizationModel>(
            new GetOrganizationRequest
            {
                OrganizationId = organizationId
            }
        );

        return result;
    }
}