namespace Navtrack.Api.Model.Assets
{
    public class AddAssetRequestModel
    {
        public string Name { get; set; }
        public int DeviceTypeId { get; set; }
        public string DeviceId { get; set; }
    }
}