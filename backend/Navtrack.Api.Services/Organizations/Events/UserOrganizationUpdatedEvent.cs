using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Organizations.Events;

public class UserOrganizationUpdatedEvent : IEvent
{
    public string UserId { get; set; }
    public string OrganizationId { get; set; }
}