using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets.Events;

public class AssetDeletedEvent(string assetId, string organizationId) : IEvent
{
    public string AssetId { get; } = assetId;
    public string OrganizationId { get; } = organizationId;
}