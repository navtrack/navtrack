using Navtrack.Api.Models;
using Navtrack.Api.Services;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Controllers
{
    public class AssetsController : GenericController<Asset, AssetModel>
    {
        public AssetsController(IAssetService assetService) : base(assetService)
        {
        }
    }
}