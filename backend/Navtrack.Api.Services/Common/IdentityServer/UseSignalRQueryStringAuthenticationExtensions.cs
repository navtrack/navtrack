using Microsoft.AspNetCore.Builder;

namespace Navtrack.Api.Services.Common.IdentityServer;

public static class UseSignalRQueryStringAuthenticationExtensions
{
    public static IApplicationBuilder UseSignalRQueryStringAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SignalRQueryStringAuthenticationMiddleware>();
    }
}