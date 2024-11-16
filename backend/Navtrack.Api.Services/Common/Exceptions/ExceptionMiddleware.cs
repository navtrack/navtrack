using System;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Common.Mappers;

namespace Navtrack.Api.Services.Common.Exceptions;

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

            Error model = ErrorMapper.Map(exception);
            
            await httpContext.Response.WriteAsJsonAsync(model);
        }
        catch (Exception exception)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            Error model = ErrorMapper.Map(exception);

            await httpContext.Response.WriteJsonAsync(model);
        }
    }
}