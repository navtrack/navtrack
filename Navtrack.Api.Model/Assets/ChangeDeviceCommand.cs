using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Assets
{
    public class ChangeDeviceCommand : BaseCommand<ChangeDeviceRequestModel>
    {
        public int AssetId { get; set; }
    }
}