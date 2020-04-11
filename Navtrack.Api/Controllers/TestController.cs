using Microsoft.AspNetCore.Mvc;

namespace Navtrack.Api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ContentResult Get()
        {
            return Content("it works");
        }
    }
}