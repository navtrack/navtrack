using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Navtrack.Api.Services.Common.Mappers;

namespace Navtrack.Api.Shared;

public class CustomClientErrorFactory(ProblemDetailsFactory problemDetailsFactory) : IClientErrorFactory
{
    public IActionResult GetClientError(ActionContext actionContext, IClientErrorActionResult clientError)
    {
        ProblemDetails problemDetails =
            problemDetailsFactory.CreateProblemDetails(actionContext.HttpContext, clientError.StatusCode);

        ProblemDetails model = ProblemDetailsMapper.Map(problemDetails);

        return new BadRequestObjectResult(model);
    }
}