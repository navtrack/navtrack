using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;

namespace Navtrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IRepository repository;

        public LocationsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<Location>> Get(string imei)
        {
            Location location = await repository.GetEntities<Location>()
                .FirstOrDefaultAsync(x => x.Device.IMEI == imei);

            return location;
        }
    }
}