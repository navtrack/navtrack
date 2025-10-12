using Navtrack.Api.Model.Assets;

namespace Navtrack.Api.Services.Assets;

public class CreateAssetRequest
{
    public string OrganizationId { get; set; }
    public CreateAssetModel Model { get; set; }
}