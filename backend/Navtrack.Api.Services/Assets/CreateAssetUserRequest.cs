using Navtrack.Api.Model.Assets;

namespace Navtrack.Api.Services.Assets;

public class CreateAssetUserRequest
{
    public string AssetId { get; set; }
    public CreateAssetUser Model { get; set; }
}