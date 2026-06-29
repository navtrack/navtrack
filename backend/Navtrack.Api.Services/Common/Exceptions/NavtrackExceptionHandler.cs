using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Services.Common.Mappers;

namespace Navtrack.Api.Services.Common.Exceptions;

public class NavtrackExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationApiException validationApiException)
        {
            ValidationProblemDetails model = ProblemDetailsMapper.Map(validationApiException);
            
            await httpContext.Response.WriteAsJsonAsync(model, cancellationToken);
        }
        else if (exception is ApiException apiException)
        {
            ProblemDetails model = ProblemDetailsMapper.Map(apiException);

            await httpContext.Response.WriteAsJsonAsync(model, cancellationToken);
        }
        
        return true;
    }
}
