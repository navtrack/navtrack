using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Navtrack.Api.Services.Common.Context;

public class NavtrackContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        INavtrackContextFactory navtrackContext = httpContext.RequestServices.GetRequiredService<INavtrackContextFactory>();

        await navtrackContext.CreateContext();
        
        await next(httpContext);
    }
}