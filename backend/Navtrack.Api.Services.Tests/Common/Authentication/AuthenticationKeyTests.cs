using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Navtrack.Api.Services.Common.Authentication;
using OpenIddict.Server;
using Xunit;

namespace Navtrack.Api.Services.Tests.Common.Authentication;

public class AuthenticationKeyTests
{
    [Fact]
    public async Task ProductionKeysCanValidateAndDecryptTokensAfterRestart()
    {
        Dictionary<string, string?> configuration = CreateConfiguration();
        OpenIddictServerOptions first = CreateOptions(configuration);
        OpenIddictServerOptions restarted = CreateOptions(configuration);
        JsonWebTokenHandler handler = new();
        string token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Claims = new Dictionary<string, object> { ["sub"] = "test-user" },
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = first.SigningCredentials.Single(),
            EncryptingCredentials = first.EncryptionCredentials.Single()
        });

        TokenValidationResult validation = await handler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            IssuerSigningKey = restarted.SigningCredentials.Single().Key,
            TokenDecryptionKey = restarted.EncryptionCredentials.Single().Key,
            ValidateIssuer = false,
            ValidateAudience = false
        });

        Assert.True(validation.IsValid, validation.Exception?.Message);
        Assert.Equal("test-user", validation.Claims["sub"]);
        Assert.Equal(first.SigningCredentials.Single().Key.KeyId, restarted.SigningCredentials.Single().Key.KeyId);
        Assert.Equal(first.EncryptionCredentials.Single().Key.KeyId, restarted.EncryptionCredentials.Single().Key.KeyId);
    }

    [Theory]
    [InlineData("Signing")]
    [InlineData("Encryption")]
    public void MissingProductionKeyReportsTheEnvironmentVariable(string purpose)
    {
        Dictionary<string, string?> configuration = CreateConfiguration();
        configuration.Remove($"Authentication:{purpose}Key");

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => CreateOptions(configuration));

        Assert.Contains($"Authentication__{purpose}Key", exception.Message);
    }

    [Theory]
    [InlineData("Signing")]
    [InlineData("Encryption")]
    public void InvalidBase64DoesNotExposeTheValue(string purpose)
    {
        Dictionary<string, string?> configuration = CreateConfiguration();
        configuration[$"Authentication:{purpose}Key"] = "private-value-not-base64";

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => CreateOptions(configuration));

        Assert.Contains("base64", exception.Message);
        Assert.DoesNotContain("private-value-not-base64", exception.ToString());
    }

    [Theory]
    [InlineData(16)]
    [InlineData(31)]
    [InlineData(33)]
    [InlineData(64)]
    public void EncryptionKeyMustBeExactly256Bits(int bytes)
    {
        Dictionary<string, string?> configuration = CreateConfiguration();
        configuration["Authentication:EncryptionKey"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(bytes));

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => CreateOptions(configuration));

        Assert.Contains("exactly 32 bytes", exception.Message);
    }

    [Fact]
    public void SigningKeyRequiresAPrivateKey()
    {
        using RSA algorithm = RSA.Create(2048);
        Dictionary<string, string?> configuration = CreateConfiguration();
        configuration["Authentication:SigningKey"] = Convert.ToBase64String(algorithm.ExportSubjectPublicKeyInfo());

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => CreateOptions(configuration));

        Assert.Contains("PKCS#8 RSA private key", exception.Message);
    }

    [Fact]
    public void SigningKeyRejectsWeakRsa()
    {
        using RSA algorithm = RSA.Create(1024);
        Dictionary<string, string?> configuration = CreateConfiguration();
        configuration["Authentication:SigningKey"] = Convert.ToBase64String(algorithm.ExportPkcs8PrivateKey());

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => CreateOptions(configuration));

        Assert.Contains("at least 2048 bits", exception.Message);
    }

    [Fact]
    public void SigningKeyRejectsTrailingData()
    {
        using RSA algorithm = RSA.Create(2048);
        Dictionary<string, string?> configuration = CreateConfiguration();
        configuration["Authentication:SigningKey"] = Convert.ToBase64String([.. algorithm.ExportPkcs8PrivateKey(), 0]);

        Assert.Throws<InvalidOperationException>(() => CreateOptions(configuration));
    }

    private static Dictionary<string, string?> CreateConfiguration()
    {
        using RSA algorithm = RSA.Create(2048);
        return new Dictionary<string, string?>
        {
            ["Authentication:SigningKey"] = Convert.ToBase64String(algorithm.ExportPkcs8PrivateKey()),
            ["Authentication:EncryptionKey"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
        };
    }

    private static OpenIddictServerOptions CreateOptions(Dictionary<string, string?> configuration)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            EnvironmentName = Environments.Production
        });
        builder.Configuration.Sources.Clear();
        builder.Configuration.AddInMemoryCollection(configuration);
        builder.Services.AddNavtrackAuthentication(builder.Configuration, builder.Environment, [],
            "auth/token", "auth/revocation");
        using IHost host = builder.Build();
        return host.Services.GetRequiredService<IOptionsMonitor<OpenIddictServerOptions>>().CurrentValue;
    }
}
