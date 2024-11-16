using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Services.DeviceMessages;

public class GetAssetMessagesRequest
{
    public string AssetId { get; set; }
    public MessageFilterModel Filter { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}