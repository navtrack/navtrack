namespace Navtrack.Api.Tests.Helpers;

public class TestWebApplicationFactoryOptions
{
    public required string ConnectionString { get; set; }
    public string? AuthenticatedUserId { get; set; }
}