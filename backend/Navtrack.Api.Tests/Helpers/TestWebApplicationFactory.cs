using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.Api.Tests.Helpers;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly TestMongoDatabaseFactory mongoDatabaseFactory;

    public TestWebApplicationFactory()
    {
        mongoDatabaseFactory = new TestMongoDatabaseFactory();
    }

    protected override IHost CreateHost(IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton<IStartupFilter, FakeRemoteIpAddressFilter>();

            ServiceDescriptor? descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(IMongoDatabaseFactory));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<IMongoDatabaseFactory>(mongoDatabaseFactory);
        });

        return base.CreateHost(hostBuilder);
    }
}