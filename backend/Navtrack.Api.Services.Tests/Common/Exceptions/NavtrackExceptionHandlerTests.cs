using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Services.Common.Exceptions;
using Xunit;

namespace Navtrack.Api.Services.Tests.Common.Exceptions;

public class NavtrackExceptionHandlerTests
{
    private readonly NavtrackExceptionHandler handler = new();

    [Fact]
    public async Task TryHandleAsync_ApiExceptionGiven_ReturnsTrue()
    {
        DefaultHttpContext httpContext = new();
        httpContext.Response.Body = new MemoryStream();

        bool result = await handler.TryHandleAsync(httpContext, new ApiException(HttpStatusCode.Forbidden),
            CancellationToken.None);

        Assert.True(result);
        Assert.Equal(StatusCodes.Status403Forbidden, httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task TryHandleAsync_UnexpectedExceptionGiven_ReturnsFalse()
    {
        DefaultHttpContext httpContext = new();

        bool result = await handler.TryHandleAsync(httpContext, new InvalidOperationException(),
            CancellationToken.None);

        Assert.False(result);
    }
}
