using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

public class AssetUserAddedEvent : IEvent
{
    public string AssetId { get; }
    public string UserId { get; }

    public AssetUserAddedEvent(string assetId, string userId)
    {
        AssetId = assetId;
        UserId = userId;
    }
}