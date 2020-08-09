namespace Navtrack.Api.Model.Assets.Requests
{
    public class ChangeDeviceRequest : BaseRequest<ChangeDeviceRequestModel>
    {
        public int UserId { get; set; }
        public int AssetId { get; set; }
    }
}