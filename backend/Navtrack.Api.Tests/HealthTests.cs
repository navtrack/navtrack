using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Navtrack.Api.Model;
using Navtrack.Api.Tests.Helpers;

namespace Navtrack.Api.Tests;

public class HealthTests(BaseTestFixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Health_Get_ReturnsOk()
    {
        HttpResponseMessage response = await HttpClient.GetAsync(ApiPaths.Health);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
