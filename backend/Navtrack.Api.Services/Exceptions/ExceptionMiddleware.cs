using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Mappers;

namespace Navtrack.Api.Services.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

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
    }
}