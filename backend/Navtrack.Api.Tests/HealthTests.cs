using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Navtrack.Api.Tests.Helpers;

namespace Navtrack.Api.Tests;

public class HealthTests(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient httpClient = factory.CreateClient();

    [Fact]
    public async Task Health_Get_ReturnsOk()
    {
        HttpResponseMessage response = await httpClient.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
