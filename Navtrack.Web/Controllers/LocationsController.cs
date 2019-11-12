using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Model;
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

        [HttpGet("{objectId}/latest")]
        public async Task<LocationModel> Get(int objectId)
        {
            LocationModel location = await locationService.GetLatestLocation(objectId);

            return location;
        }
    }
}