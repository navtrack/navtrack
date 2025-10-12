using Navtrack.Api.Model.Assets;

namespace Navtrack.Api.Services.Assets;

public class UpdateAssetRequest
{
    public string AssetId { get; set; }
    public UpdateAssetModel Model { get; set; }
}