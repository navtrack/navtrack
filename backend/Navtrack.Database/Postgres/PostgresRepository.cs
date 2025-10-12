using Microsoft.EntityFrameworkCore;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Postgres;

[Service(typeof(IPostgresRepository))]
public class PostgresRepository(IPostgresDbContextProvider postgresDbContextProvider) : IPostgresRepository
{
    public DbSet<T> GetQueryable<T>() where T : class
    {
        return postgresDbContextProvider.GetDbContext().Set<T>();
    }
    
    public DbContext GetDbContext()
    {
        return postgresDbContextProvider.GetDbContext();
    }
}