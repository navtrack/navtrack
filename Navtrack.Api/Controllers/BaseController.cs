using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Models;
using Navtrack.Api.Services.Generic;

namespace Navtrack.Api.Controllers
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