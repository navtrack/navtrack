using Navtrack.Api.Model.Organizations;

namespace Navtrack.Api.Services.Organizations;

public class CreateOrganizationUserRequest
{
    public string OrganizationId { get; set; }
    public CreateOrganizationUserModel Model { get; set; }
}