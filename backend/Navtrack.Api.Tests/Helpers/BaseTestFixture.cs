using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model;
using Testcontainers.PostgreSql;

namespace Navtrack.Api.Tests.Helpers;

public class BaseTestFixture : IAsyncLifetime
{
    protected internal TestWebApplicationFactory<Program> Factory = null!;

    private PostgreSqlContainer postgreSqlContainer;

    private const string DatabaseName = "navtrack-test";

    public async Task InitializeAsync()
    {
        postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase(DatabaseName)
            .WithUsername("navtrack-test")
            .WithPassword("navtrack-test")
            .WithImage("imresamu/postgis:17-3.5.2-alpine3.21")
            .WithCleanUp(true)
            .Build();

        await postgreSqlContainer.StartAsync();

        Factory = new TestWebApplicationFactory<Program>(new TestWebApplicationFactoryOptions
        {
            AuthenticatedUserId = DatabaseSeed.AuthenticatedUser.Id.ToString(),
            ConnectionString = postgreSqlContainer.GetConnectionString()
        });

        await SeedDatabase();
    }

    public async Task DisposeAsync()
    {
        await postgreSqlContainer.DisposeAsync();
    }

    private async Task SeedDatabase()
    {
        DbContextOptions<NavtrackDbContext> options = new DbContextOptionsBuilder<NavtrackDbContext>()
            .UseNpgsql(postgreSqlContainer.GetConnectionString(), postgresOptions =>
                postgresOptions.UseNetTopologySuite())
            .Options;

        NavtrackDbContext dbContext = new(options);
        await dbContext.Database.EnsureCreatedAsync();

        dbContext.Add(DatabaseSeed.Organization);
        dbContext.Add(DatabaseSeed.Asset);
        dbContext.Add(DatabaseSeed.Device);
        dbContext.Add(DatabaseSeed.AuthenticatedUser);

        await dbContext.SaveChangesAsync();
    }
}