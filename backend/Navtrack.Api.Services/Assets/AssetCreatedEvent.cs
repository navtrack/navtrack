using Navtrack.Api.Model.Assets;

namespace Navtrack.Api.Services.Assets;

public class AssetCreatedEvent
{
    public AssetModel Asset { get; }

    public AssetCreatedEvent(AssetModel asset)
    {
        Asset = asset;
    }
}