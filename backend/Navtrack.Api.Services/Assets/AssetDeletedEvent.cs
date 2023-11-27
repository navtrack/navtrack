namespace Navtrack.Api.Services.Assets;

public class AssetDeletedEvent
{
    public string AssetId { get; }

    public AssetDeletedEvent(string assetId)
    {
        AssetId = assetId;
    }
}