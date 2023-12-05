using Navtrack.Api.Model.Assets;

namespace Navtrack.Api.Services.Assets;

public class AssetCreatedEvent(AssetModel asset)
{
    public AssetModel Asset { get; } = asset;
}