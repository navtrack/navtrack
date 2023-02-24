using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Navtrack.Api.Services.IdentityServer;

public class SignalRQueryStringAuthenticationMiddleware
{
    private readonly RequestDelegate next;

    public SignalRQueryStringAuthenticationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.Value != null &&
            context.Request.Path.Value.Contains(ApiConstants.SignalRHubUrlPrefix) &&
            context.Request.Query.TryGetValue("access_token", out StringValues token))
        {
            context.Request.Headers.Add("Authorization", "Bearer " + token.First());
        }

        await next.Invoke(context);
    }
}