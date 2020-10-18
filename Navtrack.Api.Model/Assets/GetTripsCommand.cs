using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Assets
{
    public class GetTripsCommand : BaseCommand<GetTripsRequestModel>
    {
        public int AssetId { get; set; }
    }
}