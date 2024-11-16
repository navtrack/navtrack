using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams.Events;

public class TeamAssetDeletedEvent : IEvent
{
    public string AssetId { get; set; }
    public string TeamId { get; set; }
}