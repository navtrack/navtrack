using Navtrack.Api.Model.Devices;

namespace Navtrack.Api.Services.Devices;

public class CreateOrUpdateAssetDeviceRequest
{
    public string AssetId { get; set; }
    public CreateOrUpdateAssetDevice Model { get; set; }
}