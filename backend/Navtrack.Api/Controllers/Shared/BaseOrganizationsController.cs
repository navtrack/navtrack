using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Organizations;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers.Shared;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.Organizations)]
public abstract class BaseOrganizationsController(
    IRequestHandler requestHandler,
    INavtrackContextAccessor navtrackContextAccessor) : ControllerBase
{
    [HttpPost(ApiPaths.Organizations)]
    [ProducesResponseType(typeof(Entity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<Entity> Create([FromBody] CreateOrganization model)
    {
        Entity result = await requestHandler.Handle<CreateOrganizationRequest, Entity>(new CreateOrganizationRequest
        {
            OwnerId = navtrackContextAccessor.NavtrackContext.User.Id,
            Model = model
        });

        return result;
    }

    [HttpPost(ApiPaths.OrganizationById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Update([FromRoute] string organizationId, [FromBody] UpdateOrganizationModel model)
    {
        await requestHandler.Handle(new UpdateOrganizationRequest
        {
            OrganizationId = organizationId,
            Model = model
        });

        return Ok();
    }

    [HttpDelete(ApiPaths.OrganizationById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string organizationId)
    {
        await requestHandler.Handle(new DeleteOrganizationRequest
        {
            OrganizationId = organizationId
        });

        return Ok();
    }
}