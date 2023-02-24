using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Navtrack.Api.Tests.Helpers;

public class FakeRemoteIpAddressFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            app.UseMiddleware<FakeRemoteIpAddressMiddleware>();
            next(app);
        };
    }
}