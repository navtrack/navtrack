using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams.Events;

public class TeamDeletedEvent : IEvent
{
    public string TeamId { get; set; }
    public string OrganizationId { get; set; }
}