using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Navtrack.Api.Tests.Helpers;

public class FakeRemoteIpAddressMiddleware(RequestDelegate next)
{
    private readonly IPAddress fakeIpAddress = IPAddress.Parse("12.34.56.78");

    public async Task Invoke(HttpContext httpContext)
    {
        httpContext.Connection.RemoteIpAddress = fakeIpAddress;

        await next(httpContext);
    }
}