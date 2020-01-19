using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.DataAccess.Model;
using Navtrack.Web.Models;
using Navtrack.Web.Services;

namespace Navtrack.Web.Controllers
{
    public class AssetsController : GenericController<Asset, AssetModel>
    {
        private readonly IAssetService assetService;

        public AssetsController(IAssetService assetService) : base(assetService)
        {
            this.assetService = assetService;
        }
        
        [HttpGet("stats")]
        public Task<AssetStatsModel> GetStats()
        {
            return assetService.GetStats();
        }
    }
}