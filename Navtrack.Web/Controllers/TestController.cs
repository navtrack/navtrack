using Microsoft.AspNetCore.Mvc;

namespace Navtrack.Web.Controllers
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