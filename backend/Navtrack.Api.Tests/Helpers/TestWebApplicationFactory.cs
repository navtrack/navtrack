using System.Linq;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.Api.Tests.Helpers;

public class TestWebApplicationFactory<TProgram>(TestWebApplicationFactoryOptions options)
    : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton<IStartupFilter, FakeRemoteIpAddressFilter>();

            ReplaceMongoOptions(services);

            if (!string.IsNullOrEmpty(options.AuthenticatedUserId))
            {
                services.AddSingleton<IPolicyEvaluator>(new FakePolicyEvaluator(options.AuthenticatedUserId));
            }
        });

        return base.CreateHost(hostBuilder);
    }

    private  void ReplaceMongoOptions(IServiceCollection services)
    {
        ServiceDescriptor? descriptor =
            services.SingleOrDefault(d => d.ServiceType == typeof(IOptions<MongoOptions>));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        MongoOptions mongoOptions = new()
        {
            ConnectionString = "mongodb://localhost:27017",
            Database = options.DatabaseName
        };
        services.AddSingleton<IOptions<MongoOptions>>(new OptionsWrapper<MongoOptions>(mongoOptions));
    }
}