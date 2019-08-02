using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Navtrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public TestController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            string connectionString = configuration.GetConnectionString("navtrack");

            return connectionString;
        }
    }
}