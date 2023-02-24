namespace Navtrack.Api.Tests.Helpers;

public class BaseTest
{
    protected readonly TestWebApplicationFactory<Program> factory;
    protected readonly HttpClient httpClient;

    protected BaseTest()
    {
        factory = new TestWebApplicationFactory<Program>();
        httpClient = factory.CreateClient();
    }
}