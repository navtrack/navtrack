using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Assets
{
    public class GetLocationsCommand : BaseCommand<LocationHistoryRequestModel>
    {
        public int AssetId { get; set; }
    }
}