using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

public class AssetCreatedEvent(string assetId) : IEvent
{
    public string AssetId { get; } = assetId;
}