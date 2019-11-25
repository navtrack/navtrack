using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
using Navtrack.Web.Services;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService assetService;

        public AssetsController(IAssetService assetService)
        {
            this.assetService = assetService;
        }

        [HttpGet("{id}")]
        public Task<AssetModel> Get(int id)
        {
            return assetService.Get(id);
        }

        [HttpGet]
        public Task<List<AssetModel>> GetAll()
        {
            return assetService.GetAll();
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(AssetModel asset)
        {
            await assetService.ValidateModel(asset, ModelState);
            
            if (ModelState.IsValid)
            {
                await assetService.Add(asset);

                return Ok();
            }
            
            return ValidationProblem();
        }
    }
}