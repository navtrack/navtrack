using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Navtrack.Api.Services.Mappers;

namespace Navtrack.Api.Services.Exceptions;

public class ModelStateMappingActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new JsonResult(ErrorModelMapper.Map(context.ModelState))
            {
                StatusCode = (int?)HttpStatusCode.BadRequest
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}