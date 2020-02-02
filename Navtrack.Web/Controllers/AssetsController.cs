using Navtrack.DataAccess.Model;
using Navtrack.Web.Models;
using Navtrack.Web.Services;

namespace Navtrack.Web.Controllers
{
    public class AssetsController : GenericController<Asset, AssetModel>
    {
        public AssetsController(IAssetService assetService) : base(assetService)
        {
        }
    }
}