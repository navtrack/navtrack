using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.Api.Tests.Helpers;

public class BaseTestFixtureOptions
{
    public string? AuthenticatedUserId { get; set; } 
}

public class BaseTestFixture : IDisposable
{
    protected internal TestWebApplicationFactory<Program> Factory;
    protected internal HttpClient HttpClient;
    public bool DatabaseSeeded { get; set; }

    public void Initialize(BaseTestFixtureOptions options)
    {
        Factory = new TestWebApplicationFactory<Program>(new TestWebApplicationFactoryOptions
        {
            AuthenticatedUserId = options.AuthenticatedUserId
        });
        HttpClient = Factory.CreateClient();
    }

    public void Dispose()
    {
        IServiceScope serviceProvider = Factory.Services.CreateScope();
        IMongoDatabaseFactory? mongoDatabaseFactory =
            serviceProvider.ServiceProvider.GetService<IMongoDatabaseFactory>();
        mongoDatabaseFactory?.DropDatabase();
    }
}