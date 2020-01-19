using System.Collections.Generic;
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

        [HttpGet("{assetId}/history")]
        public async Task<List<LocationModel>> GetHistory(int assetId)
        {
            List<LocationModel> locations = await locationService.GetLocations(assetId);

            return locations;
        }
        
        [HttpGet("{assetId}/latest")]
        public async Task<LocationModel> GetLatest(int assetId)
        {
            LocationModel location = await locationService.GetLatestLocation(assetId);

            return location;
        }
    }
}