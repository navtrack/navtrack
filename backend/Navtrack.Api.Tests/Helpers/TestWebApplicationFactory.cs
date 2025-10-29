using System.Linq;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Database.Model;

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

            if (!string.IsNullOrEmpty(options.AuthenticatedUserId))
            {
                services.AddSingleton<IPolicyEvaluator>(new FakePolicyEvaluator(options.AuthenticatedUserId));
            }

            ServiceDescriptor? dbContext = services.FirstOrDefault(x => x.ServiceType == typeof(DbContext));
            if (dbContext != null)
            {
                services.Remove(dbContext);
            }

            DbContextUtils.AddDbContext<NavtrackDbContext>(services, options.ConnectionString);
        });

        return base.CreateHost(hostBuilder);
    }
}