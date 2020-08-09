namespace Navtrack.Api.Model.Assets.Requests
{
    public class RenameAssetRequest : BaseRequest<RenameAssetRequestModel>
    {
        public int AssetId { get; set; }
        public int UserId { get; set; }
    }
}