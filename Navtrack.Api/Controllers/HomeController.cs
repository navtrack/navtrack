using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Navtrack.Api.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult<string> Index()
        {
            return "it works";
        }
    }
}