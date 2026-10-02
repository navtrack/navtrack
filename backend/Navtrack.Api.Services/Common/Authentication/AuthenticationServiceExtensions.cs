using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    options.AddSigningCertificate(LoadCertificate(configuration, "Signing"));
                    options.AddEncryptionCertificate(LoadCertificate(configuration, "Encryption"));
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

    private static X509Certificate2 LoadCertificate(IConfiguration configuration, string name)
    {
        string path = configuration[$"Authentication:{name}CertificatePath"] ??
            throw new InvalidOperationException($"Authentication:{name}CertificatePath is required outside development.");
        X509Certificate2 certificate = X509CertificateLoader.LoadPkcs12FromFile(path,
            configuration[$"Authentication:{name}CertificatePassword"], X509KeyStorageFlags.EphemeralKeySet);
        if (!certificate.HasPrivateKey)
            throw new InvalidOperationException($"The authentication {name.ToLowerInvariant()} certificate must contain a private key.");
        return certificate;
    }
}