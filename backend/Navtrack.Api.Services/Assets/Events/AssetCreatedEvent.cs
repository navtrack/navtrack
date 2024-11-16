using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets.Events;

public class AssetCreatedEvent(string assetId, string organizationId) : IEvent
{
    public string AssetId { get; } = assetId;
    public string OrganizationId { get; } = organizationId;
}