using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Organizations.Events;

public class UserOrganizationDeletedEvent : IEvent
{
    public string UserId { get; set; }
    public string OrganizationId { get; set; }
}