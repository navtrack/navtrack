namespace Navtrack.Api.Model.Assets
{
    public class GetAssetByIdCommand 
    {
        public int AssetId { get; set; }
        public int UserId { get; set; }
    }
}