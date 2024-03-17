using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

public class AssetUserDeletedEvent : IEvent
{
    public string AssetId { get; }
    public string UserId { get; }

    public AssetUserDeletedEvent(string assetId, string userId)
    {
        AssetId = assetId;
        UserId = userId;
    }
}