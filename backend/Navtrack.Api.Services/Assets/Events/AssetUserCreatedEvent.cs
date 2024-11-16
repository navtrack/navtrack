using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets.Events;

public class AssetUserCreatedEvent : IEvent
{
    public string AssetId { get; }
    public string UserId { get; }

    public AssetUserCreatedEvent(string assetId, string userId)
    {
        AssetId = assetId;
        UserId = userId;
    }
}