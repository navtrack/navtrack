using System;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Mappers.Common;

namespace Navtrack.Api.Services.Exceptions;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (ApiException exception)
        {
            httpContext.Response.StatusCode = (int)exception.HttpStatusCode;

            ErrorModel model = ErrorModelMapper.Map(exception);

            await httpContext.Response.WriteAsJsonAsync(model);
        }
        catch (Exception exception)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ErrorModel model = ErrorModelMapper.Map(exception);

            await httpContext.Response.WriteJsonAsync(model);
        }
    }
}