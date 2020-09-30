using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Assets
{
    public class RenameAssetCommand : BaseCommand<RenameAssetRequestModel>
    {
        public int AssetId { get; set; }
    }
}