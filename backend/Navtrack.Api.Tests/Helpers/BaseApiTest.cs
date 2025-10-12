using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Database.Postgres;

namespace Navtrack.Api.Tests.Helpers;

public class BaseApiTest : IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture fixture;
    protected bool DisableAuthentication { get; set; }

    protected BaseApiTest(BaseTestFixture fixture)
    {
        this.fixture = fixture; 

        SeedDatabase();
    }
    
    private HttpClient? httpClient;
    protected HttpClient HttpClient => httpClient ??= fixture.Factory.CreateClient();

    private IServiceProvider? serviceProvider;
    protected IServiceProvider ServiceProvider =>
        serviceProvider ??= fixture.Factory.Services.CreateScope().ServiceProvider;

    protected IPostgresRepository Repository => ServiceProvider.GetService<IPostgresRepository>()!;

    protected virtual void SeedDatabase()
    {
    }

    protected static string GetUrl(string path, params KeyValuePair<string, string>[] values)
    {
        return values.Aggregate(path,
            (current, pair) =>
                current.Replace($"{{{pair.Key}}}", pair.Value, StringComparison.InvariantCultureIgnoreCase));
    }
}