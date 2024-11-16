using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams.Events;

public class TeamUserDeletedEvent : IEvent
{
    public string UserId { get; set; }
    public string TeamId { get; set; }
}