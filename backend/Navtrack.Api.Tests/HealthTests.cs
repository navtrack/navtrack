using System.Net;
using Navtrack.Api.Tests.Helpers;

namespace Navtrack.Api.Tests;

public class HealthTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient httpClient;

    public HealthTests(TestWebApplicationFactory<Program> factory)
    {
        httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Health_Get_ReturnsOk()
    {
        HttpResponseMessage response = await httpClient.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
