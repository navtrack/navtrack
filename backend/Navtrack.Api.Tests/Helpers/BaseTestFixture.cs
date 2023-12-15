using System;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.Api.Tests.Helpers;

public class BaseTestFixture : IDisposable
{
    protected internal TestWebApplicationFactory<Program> Factory = null!;
    public bool DatabaseSeeded { get; set; }
    private readonly string databaseGuid = Guid.NewGuid().ToString();

    public void Initialize(BaseTestFixtureOptions options)
    {
        Factory = new TestWebApplicationFactory<Program>(new TestWebApplicationFactoryOptions
        {
            AuthenticatedUserId = options.AuthenticatedUserId,
            DatabaseName = $"navtrack-test-{databaseGuid:N}"
        });
    }

    public void Dispose()
    {
        IServiceScope serviceProvider = Factory.Services.CreateScope();
        IMongoDatabaseProvider? mongoDatabaseFactory =
            serviceProvider.ServiceProvider.GetService<IMongoDatabaseProvider>();
        mongoDatabaseFactory?.DropDatabase();
    }
}