using Navtrack.Api.Model.Organizations;

namespace Navtrack.Api.Services.Organizations;

public class UpdateOrganizationRequest
{
    public string OrganizationId { get; set; }
    public UpdateOrganizationModel Model { get; set; }
}