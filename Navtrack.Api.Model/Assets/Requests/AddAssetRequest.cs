namespace Navtrack.Api.Model.Assets.Requests
{
    public class AddAssetRequest : BaseRequest<AddAssetRequestModel>
    {
        public int UserId { get; set; }
    }
}