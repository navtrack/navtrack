using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.Api.Tests.Helpers;

public class FakePolicyEvaluator(string? userId) : IPolicyEvaluator
{
    public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(new[]
        {
            new Claim(JwtClaimTypes.Subject, userId)
        }, "TestScheme"));

        return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal,
            new AuthenticationProperties(), "TestScheme")));
    }

    public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
        AuthenticateResult authenticationResult, HttpContext context, object resource)
    {
        return await Task.FromResult(PolicyAuthorizationResult.Success());
    }
}

public class TestWebApplicationFactoryOptions
{
    public string? AuthenticatedUserId { get; set; }
}

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly TestWebApplicationFactoryOptions options;

    public TestWebApplicationFactory(TestWebApplicationFactoryOptions options)
    {
        this.options = options;
    }

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

    private static void ReplaceMongoOptions(IServiceCollection services)
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
            Database = $"navtrack-test-{Guid.NewGuid():N}"
        };
        services.AddSingleton<IOptions<MongoOptions>>(new OptionsWrapper<MongoOptions>(mongoOptions));
    }
}