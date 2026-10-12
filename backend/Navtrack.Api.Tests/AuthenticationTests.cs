using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Authentication;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Tests.Helpers;
using Navtrack.Database.Model;
using Navtrack.Database.Model.Users;
using Testcontainers.PostgreSql;

namespace Navtrack.Api.Tests;

// Exercises the real protocol middleware and PostgreSQL token stores, without FakePolicyEvaluator.
public class AuthenticationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer database = new PostgreSqlBuilder()
        .WithImage("imresamu/postgis:17-3.5.2-alpine3.21")
        .WithDatabase("navtrack-auth-test")
        .WithUsername("navtrack-test")
        .WithPassword("navtrack-test")
        .Build();
    private WebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;

    public async Task InitializeAsync()
    {
        await database.StartAsync();
        DbContextOptions<NavtrackDbContext> options = new DbContextOptionsBuilder<NavtrackDbContext>()
            .UseNpgsql(database.GetConnectionString(), postgres => postgres.UseNetTopologySuite()).Options;
        await using NavtrackDbContext context = new NavtrackDbContext(options);
        await context.Database.MigrateAsync();
        (string hash, string salt) = new PasswordHasher().Hash("test-password");
        Guid userId = Guid.NewGuid();
        context.Users.Add(new UserEntity
        {
            Id = userId, Email = "auth@example.test", PasswordHash = hash, PasswordSalt = salt
        });
        context.Users.Add(new UserEntity { Id = Guid.NewGuid(), Email = "social@example.test" });
        await context.SaveChangesAsync();
        factory = new TestWebApplicationFactory<Program>(new TestWebApplicationFactoryOptions
        {
            ConnectionString = database.GetConnectionString()
        }).WithWebHostBuilder(builder => builder.ConfigureServices(services =>
        {
            services.AddSingleton(new AuthenticationClientConfiguration(["test-provider"]));
            services.Configure<OpenIddictServerOptions>(options => options.GrantTypes.Add("test-provider"));
            services.AddSingleton<IAuthenticationGrantHandler>(new TestProviderGrantHandler(userId.ToString()));
        }));
        client = factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        client?.Dispose();
        if (factory != null) await factory.DisposeAsync();
        await database.DisposeAsync();
    }

    [Fact]
    public async Task LoginRefreshAndLogoutRevokeTheEntireSession()
    {
        JsonElement login = await SuccessfulToken(PasswordRequest());
        string firstAccessToken = login.GetProperty("access_token").GetString()!;
        string firstRefreshToken = login.GetProperty("refresh_token").GetString()!;
        Dictionary<string, string> refreshRequest = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token", ["client_id"] = "navtrack.web", ["refresh_token"] = firstRefreshToken
        };
        JsonElement refresh = await SuccessfulToken(refreshRequest);
        Assert.NotEqual(firstRefreshToken, refresh.GetProperty("refresh_token").GetString());
        string accessToken = refresh.GetProperty("access_token").GetString()!;
        HttpRequestMessage logout = new HttpRequestMessage(HttpMethod.Post, "/" + ApiPaths.AuthLogout);
        logout.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        Assert.Equal(HttpStatusCode.NoContent, (await client.SendAsync(logout)).StatusCode);

        foreach (string token in new[] { firstAccessToken, accessToken })
        {
            HttpRequestMessage rejected = new HttpRequestMessage(HttpMethod.Post, "/" + ApiPaths.AuthLogout);
            rejected.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Assert.Equal(HttpStatusCode.Unauthorized, (await client.SendAsync(rejected)).StatusCode);
        }
        refreshRequest["refresh_token"] = refresh.GetProperty("refresh_token").GetString()!;
        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(refreshRequest))).StatusCode);
    }

    [Theory]
    [InlineData("auth@example.test", "incorrect")]
    [InlineData("social@example.test", "anything")]
    [InlineData("missing@example.test", "anything")]
    public async Task InvalidPasswordPreservesApplicationError(string email, string password)
    {
        Dictionary<string, string> request = PasswordRequest();
        request["username"] = email;
        request["password"] = password;
        HttpResponseMessage response = await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        JsonElement body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("invalid_grant", body.GetProperty("error").GetString());
        Assert.True(body.TryGetProperty("code", out _));
    }

    [Fact]
    public async Task RefreshTokensAreBoundToTheirClient()
    {
        JsonElement login = await SuccessfulToken(PasswordRequest());
        Dictionary<string, string> request = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token", ["client_id"] = "navtrack.mobile",
            ["refresh_token"] = login.GetProperty("refresh_token").GetString()!
        };
        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request))).StatusCode);
    }

    [Fact]
    public async Task TokensWithoutApiScopeCanAccessApi()
    {
        Dictionary<string, string> request = PasswordRequest();
        request["scope"] = "openid offline_access";
        JsonElement login = await SuccessfulToken(request);
        HttpRequestMessage logout = new HttpRequestMessage(HttpMethod.Post, "/" + ApiPaths.AuthLogout);
        logout.Headers.Authorization = new AuthenticationHeaderValue("Bearer", login.GetProperty("access_token").GetString());
        Assert.Equal(HttpStatusCode.NoContent, (await client.SendAsync(logout)).StatusCode);
    }

    [Theory]
    [InlineData("unknown-client", "password")]
    [InlineData("navtrack.web", "google")]
    public async Task RejectsUnregisteredClientsAndGrants(string clientId, string grantType)
    {
        Dictionary<string, string> request = PasswordRequest();
        request["client_id"] = clientId;
        request["grant_type"] = grantType;
        HttpResponseMessage response = await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request));
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task LegacyRefreshTokensAreRejected()
    {
        Dictionary<string, string> request = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token", ["client_id"] = "navtrack.web",
            ["refresh_token"] = "legacy-identityserver-handle"
        };
        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request))).StatusCode);
    }

    [Fact]
    public async Task ExpiredRefreshTokenIsRejected()
    {
        JsonElement login = await SuccessfulToken(PasswordRequest());
        using IServiceScope scope = factory.Services.CreateScope();
        DbContext context = scope.ServiceProvider.GetRequiredService<DbContext>();
        int updated = await context.Set<OpenIddictEntityFrameworkCoreToken>()
            .Where(token => token.Type == OpenIddictConstants.TokenTypeIdentifiers.RefreshToken)
            .ExecuteUpdateAsync(update => update.SetProperty(token => token.ExpirationDate, DateTime.UtcNow.AddMinutes(-1)));
        Assert.Equal(1, updated);
        Dictionary<string, string> request = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token", ["client_id"] = "navtrack.web",
            ["refresh_token"] = login.GetProperty("refresh_token").GetString()!
        };
        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request))).StatusCode);
    }

    [Fact]
    public async Task RefreshReplayOutsideConcurrencyAllowanceRevokesTheSession()
    {
        JsonElement login = await SuccessfulToken(PasswordRequest());
        Dictionary<string, string> request = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token", ["client_id"] = "navtrack.web",
            ["refresh_token"] = login.GetProperty("refresh_token").GetString()!
        };
        JsonElement refreshed = await SuccessfulToken(request);
        using IServiceScope scope = factory.Services.CreateScope();
        DbContext context = scope.ServiceProvider.GetRequiredService<DbContext>();
        int updated = await context.Set<OpenIddictEntityFrameworkCoreToken>()
            .Where(token => token.Type == OpenIddictConstants.TokenTypeIdentifiers.RefreshToken && token.Status == OpenIddictConstants.Statuses.Redeemed)
            .ExecuteUpdateAsync(update => update.SetProperty(token => token.RedemptionDate, DateTime.UtcNow.AddMinutes(-1)));
        Assert.Equal(1, updated);
        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request))).StatusCode);
        request["refresh_token"] = refreshed.GetProperty("refresh_token").GetString()!;
        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request))).StatusCode);
    }

    [Fact]
    public async Task CustomGrantIssuesTokensThroughTheProtocolPipeline()
    {
        Dictionary<string, string> request = PasswordRequest();
        request["grant_type"] = "test-provider";
        request["code"] = "provider-token";
        JsonElement result = await SuccessfulToken(request);
        Assert.False(string.IsNullOrEmpty(result.GetProperty("refresh_token").GetString()));
    }

    [Fact]
    public async Task CustomGrantLinkingErrorPreservesCodeAndTokenOnTheWire()
    {
        Dictionary<string, string> request = PasswordRequest();
        request["grant_type"] = "test-provider";
        request["code"] = "link";
        HttpResponseMessage response = await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        JsonElement result = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal(ApiErrorCodes.Login_SocialLoginNotLinked.Code, result.GetProperty("code").GetString());
        Assert.Equal("linking-token", result.GetProperty("token").GetString());
    }

    private sealed class TestProviderGrantHandler(string userId) : IAuthenticationGrantHandler
    {
        public string GrantType => "test-provider";
        public Task<AuthenticationGrantResult> Authenticate(OpenIddictRequest request) => Task.FromResult(
            request.Code == "link"
                ? new AuthenticationGrantResult(Error: ApiErrorCodes.Login_SocialLoginNotLinked, LinkingToken: "linking-token")
                : new AuthenticationGrantResult(UserId: userId));
    }

    private async Task<JsonElement> SuccessfulToken(Dictionary<string, string> request)
    {
        HttpResponseMessage response = await client.PostAsync("/" + ApiPaths.AuthToken, new FormUrlEncodedContent(request));
        string body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, body);
        return JsonDocument.Parse(body).RootElement.Clone();
    }

    private static Dictionary<string, string> PasswordRequest() => new()
    {
        ["grant_type"] = "password", ["client_id"] = "navtrack.web", ["username"] = "auth@example.test",
        ["password"] = "test-password", ["scope"] = "openid offline_access"
    };
}
