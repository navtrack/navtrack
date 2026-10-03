using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace Navtrack.Api.Services.Common.Authentication;

public static class AuthenticationServiceExtensions
{
    public static void AddNavtrackAuthentication(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment, string[] customGrantTypes, string tokenEndpoint, string revocationEndpoint)
    {
        services.AddSingleton(new AuthenticationClientConfiguration(customGrantTypes));
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<DbContext>();
                // Revocations must be visible immediately on every API replica.
                options.DisableEntityCaching();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris(tokenEndpoint);
                options.SetRevocationEndpointUris(revocationEndpoint);
                options.AllowPasswordFlow().AllowRefreshTokenFlow();
                foreach (string grant in customGrantTypes)
                {
                    options.AllowCustomFlow(grant);
                }

                options.RegisterScopes(OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.OfflineAccess);
                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(60));
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(365));
                // Preserve the previous maximum session lifetime, even when refreshed regularly.
                options.DisableSlidingRefreshTokenExpiration();
                options.SetRefreshTokenReuseLeeway(TimeSpan.FromSeconds(30));
                // Keep JWT access tokens readable by API tooling. Refresh tokens remain encrypted.
                options.DisableAccessTokenEncryption();

                string? issuer = configuration["Authentication:Issuer"];
                if (!string.IsNullOrEmpty(issuer)) options.SetIssuer(new Uri(issuer));

                if (environment.IsEnvironment("NSwag") || environment.IsEnvironment("Testing"))
                {
                    options.AddEphemeralSigningKey().AddEphemeralEncryptionKey();
                }
                else if (environment.IsDevelopment())
                {
                    options.AddDevelopmentSigningCertificate().AddDevelopmentEncryptionCertificate();
                }
                else
                {
                    options.AddSigningKey(LoadSigningKey(configuration));
                    options.AddEncryptionKey(LoadEncryptionKey(configuration));
                }

                OpenIddictServerAspNetCoreBuilder host = options.UseAspNetCore().EnableTokenEndpointPassthrough();
                if (environment.IsDevelopment() || environment.IsEnvironment("Testing"))
                {
                    host.DisableTransportSecurityRequirement();
                }
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
                options.AddAudiences(AuthenticationConstants.ApiAudience);
                options.EnableTokenEntryValidation();
                options.EnableAuthorizationEntryValidation();
            });

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(
                    OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        if (!environment.IsEnvironment("NSwag")) services.AddHostedService<AuthenticationClientInitializer>();
    }

    private static RsaSecurityKey LoadSigningKey(IConfiguration configuration)
    {
        byte[] key = LoadKey(configuration, "Signing");
        try
        {
            using RSA algorithm = RSA.Create();
            algorithm.ImportPkcs8PrivateKey(key, out int bytesRead);
            if (bytesRead != key.Length || algorithm.KeySize < 2048)
            {
                throw new InvalidOperationException(
                    "Authentication:SigningKey must contain a single PKCS#8 RSA private key of at least 2048 bits.");
            }

            return new RsaSecurityKey(algorithm.ExportParameters(includePrivateParameters: true))
            {
                KeyId = Base64UrlEncoder.Encode(SHA256.HashData(algorithm.ExportSubjectPublicKeyInfo()))
            };
        }
        catch (CryptographicException)
        {
            throw new InvalidOperationException("Authentication:SigningKey must be a base64-encoded PKCS#8 RSA private key.");
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }
    }

    private static SymmetricSecurityKey LoadEncryptionKey(IConfiguration configuration)
    {
        byte[] key = LoadKey(configuration, "Encryption");
        if (key.Length != 32)
        {
            CryptographicOperations.ZeroMemory(key);
            throw new InvalidOperationException("Authentication:EncryptionKey must contain exactly 32 bytes (256 bits).");
        }

        return new SymmetricSecurityKey(key)
        {
            KeyId = Base64UrlEncoder.Encode(SHA256.HashData(key))
        };
    }

    private static byte[] LoadKey(IConfiguration configuration, string name)
    {
        string? value = configuration[$"Authentication:{name}Key"];
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Authentication:{name}Key is required outside development. Set Authentication__{name}Key in the environment.");
        }

        try
        {
            return Convert.FromBase64String(value);
        }
        catch (FormatException)
        {
            throw new InvalidOperationException($"Authentication:{name}Key must be base64-encoded.");
        }
    }
}
