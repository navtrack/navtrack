namespace Navtrack.Api.Services.Organizations;

public class DeleteOrganizationUserRequest
{
    public string OrganizationId { get; set; }
    public string UserId { get; set; }
}