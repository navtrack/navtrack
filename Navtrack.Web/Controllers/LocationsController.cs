using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
using Navtrack.Web.Services;

namespace Navtrack.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService locationService;

        public LocationsController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet("latest/{assetId}")]
        public async Task<LocationModel> Get(int assetId)
        {
            LocationModel location = await locationService.GetLatestLocation(assetId);

            return location;
        }
    }
}