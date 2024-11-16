using Navtrack.Api.Model.Organizations;

namespace Navtrack.Api.Services.Organizations;

public class UpdateOrganizationUserRequest
{
    public string OrganizationId { get; set; }
    public string UserId { get; set; }
    public UpdateOrganizationUser Model { get; set; }
}