namespace Navtrack.Api.Model.Locations.Requests
{
    public class GetLatestLocationRequest
    {
        public int UserId { get; set; }
        public int AssetId { get; set; }
    }
}