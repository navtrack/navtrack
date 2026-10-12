using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Navtrack.Api.Services.Common.Authentication;

// Database migrations must run before the application starts accepting traffic.
public class AuthenticationClientInitializer(IServiceScopeFactory scopeFactory,
    AuthenticationClientConfiguration configuration) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DbContext database = scope.ServiceProvider.GetRequiredService<DbContext>();
        await using IDbContextTransaction transaction = await database.Database.BeginTransactionAsync(cancellationToken);
        // Serialize seeding across API replicas; the lock is released with the transaction.
        await database.Database.ExecuteSqlRawAsync("SELECT pg_advisory_xact_lock(1851880555)", cancellationToken);
        IOpenIddictApplicationManager manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        foreach (string clientId in new[] { AuthenticationConstants.WebClientId, AuthenticationConstants.MobileClientId })
        {
            OpenIddictApplicationDescriptor descriptor = new()
            {
                ClientId = clientId,
                ClientType = ClientTypes.Public,
                ConsentType = ConsentTypes.Implicit,
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.Revocation,
                    Permissions.GrantTypes.Password,
                    Permissions.GrantTypes.RefreshToken
                }
            };
            foreach (string grant in configuration.CustomGrantTypes)
                descriptor.Permissions.Add(Permissions.Prefixes.GrantType + grant);

            object? application = await manager.FindByClientIdAsync(clientId, cancellationToken);
            if (application == null) await manager.CreateAsync(descriptor, cancellationToken);
            else await manager.UpdateAsync(application, descriptor, cancellationToken);
        }
        await transaction.CommitAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
