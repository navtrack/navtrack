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
public class OrganizationsUsersController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.OrganizationUsers)]
    [ProducesResponseType(typeof(ListModel<OrganizationUserModel>), StatusCodes.Status200OK)]
    [NavtrackAuthorize(OrganizationUserRole.Member)]
    public Task<ListModel<OrganizationUserModel>> List([FromRoute] string organizationId) =>
        Query<GetOrganizationUsersRequest, ListModel<OrganizationUserModel>>(new GetOrganizationUsersRequest
        {
            OrganizationId = organizationId
        });

    [HttpPost(ApiPaths.OrganizationUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Create([FromRoute] string organizationId, [FromBody] CreateOrganizationUserModel model) =>
        await Command(new CreateOrganizationUserRequest
        {
            OrganizationId = organizationId,
            Model = model
        });

    [HttpPost(ApiPaths.OrganizationUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Update([FromRoute] string organizationId, [FromRoute] string userId,
        [FromBody] UpdateOrganizationUserModel model) =>
        await Command(new UpdateOrganizationUserRequest
        {
            OrganizationId = organizationId,
            UserId = userId,
            Model = model
        });

    [HttpDelete(ApiPaths.OrganizationUserById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(OrganizationUserRole.Owner)]
    public async Task<IActionResult> Delete([FromRoute] string organizationId, [FromRoute] string userId) =>
        await Command(new DeleteOrganizationUserRequest
        {
            OrganizationId = organizationId,
            UserId = userId
        });
}
