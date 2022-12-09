using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Navtrack.Api.Services.Mappers;

namespace Navtrack.Api.Services.ActionFilters;

public class ModelStateMappingActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new JsonResult(ErrorModelMapper.Map(context.ModelState))
            {
                StatusCode = (int?)HttpStatusCode.BadRequest
            };
        }
    }
}