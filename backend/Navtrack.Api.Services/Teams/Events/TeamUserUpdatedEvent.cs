using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams.Events;

public class TeamUserUpdatedEvent : IEvent
{
    public string UserId { get; set; }
    public string TeamId { get; set; }
}