using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Navtrack.Api.Services.Common.RequestContext;

public class NavtrackRequestContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        INavtrackRequestContextFactory navtrackRequestContext = httpContext.RequestServices.GetRequiredService<INavtrackRequestContextFactory>();

        await navtrackRequestContext.CreateContext();
        
        await next(httpContext);
    }
}