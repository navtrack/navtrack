using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Assets
{
    public class GetLocationsCommand : BaseCommand<LocationHistoryRequestModel>
    {
        public int UserId { get; set; }
        public int AssetId { get; set; }
    }
}