using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

public class AssetDeletedEvent(string assetId) : IEvent
{
    public string AssetId { get; } = assetId;
}