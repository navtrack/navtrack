using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams.Events;

public class TeamAssetCreatedEvent : IEvent
{
    public string AssetId { get; set; }
    public string TeamId { get; set; }
}