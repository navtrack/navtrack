using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Assets
{
    public class GetLocationsCommand : BaseCommand<GetLocationsRequestModel>
    {
        public int AssetId { get; set; }
    }
}