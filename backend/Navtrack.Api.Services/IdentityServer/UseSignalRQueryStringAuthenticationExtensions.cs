using Microsoft.AspNetCore.Builder;

namespace Navtrack.Api.Services.IdentityServer;

public static class UseSignalRQueryStringAuthenticationExtensions
{
    public static IApplicationBuilder UseSignalRQueryStringAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SignalRQueryStringAuthenticationMiddleware>();
    }
}