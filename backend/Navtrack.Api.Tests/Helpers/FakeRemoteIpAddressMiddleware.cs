using System.Net;
using Microsoft.AspNetCore.Http;

namespace Navtrack.Api.Tests.Helpers;

public class FakeRemoteIpAddressMiddleware
{
    private readonly RequestDelegate next;
    private readonly IPAddress fakeIpAddress = IPAddress.Parse("12.34.56.78");

    public FakeRemoteIpAddressMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        httpContext.Connection.RemoteIpAddress = fakeIpAddress;

        await next(httpContext);
    }
}