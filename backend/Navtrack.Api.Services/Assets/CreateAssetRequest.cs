using Navtrack.Api.Model.Assets;

namespace Navtrack.Api.Services.Assets;

public class CreateAssetRequest
{
    public string OrganizationId { get; set; }
    public CreateAsset Model { get; set; }
}