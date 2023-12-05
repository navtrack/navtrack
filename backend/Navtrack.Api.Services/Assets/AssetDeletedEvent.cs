namespace Navtrack.Api.Services.Assets;

public class AssetDeletedEvent(string assetId)
{
    public string AssetId { get; } = assetId;
}