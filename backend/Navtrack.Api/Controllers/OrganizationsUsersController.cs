using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Organizations;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.Users)]
public class OrganizationsUsersController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.OrganizationUsers)]
    [ProducesResponseType(typeof(ListModel<OrganizationUserModel>), StatusCodes.Status200OK)]
    [AuthorizeOrganization(OrganizationUserRole.Member)]
    public async Task<ListModel<OrganizationUserModel>> List([FromRoute] string organizationId)
    {
        ListModel<OrganizationUserModel> result =
            await requestHandler.Handle<GetOrganizationUsersRequest, ListModel<OrganizationUserModel>>(
                new GetOrganizationUsersRequest
                {
                    OrganizationId = organizationId
                });

        return result;
    }

    [HttpPost(ApiPaths.OrganizationUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string organizationId, [FromBody] CreateOrganizationUserModel model)
    {
        await requestHandler.Handle(new CreateOrganizationUserRequest
        {
            OrganizationId = organizationId,
            Model = model
        });

        return Ok();
    }

    [HttpPost(ApiPaths.OrganizationUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Update([FromRoute] string organizationId, [FromRoute] string userId,
        [FromBody] UpdateOrganizationUserModel model)
    {
        await requestHandler.Handle(new UpdateOrganizationUserRequest
        {
            OrganizationId = organizationId,
            UserId = userId,
            Model = model
        });

        return Ok();
    }

    [HttpDelete(ApiPaths.OrganizationUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeOrganization(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string organizationId, [FromRoute] string userId)
    {
        await requestHandler.Handle(new DeleteOrganizationUserRequest
        {
            OrganizationId = organizationId,
            UserId = userId
        });

        return Ok();
    }
}