using Microsoft.EntityFrameworkCore;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Postgres;

[Service(typeof(IPostgresDbContextProvider))]
public class PostgresDbContextProvider(DbContext dbContext) : IPostgresDbContextProvider
{
    public DbContext GetDbContext()
    {
        return dbContext;
    }
}