using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Assets
{
    public class ChangeDeviceCommand : BaseCommand<ChangeDeviceRequestModel>
    {
        public int UserId { get; set; }
        public int AssetId { get; set; }
    }
}