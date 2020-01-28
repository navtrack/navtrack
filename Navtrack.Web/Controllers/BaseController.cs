using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult ValidationResult(ValidationResult validationResult)
        {
            return validationResult.IsValid ? (IActionResult) Ok() : BadRequest(new ErrorModel(validationResult));
        }
    }
}