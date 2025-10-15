using Microsoft.EntityFrameworkCore;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Postgres;

[Service(typeof(IPostgresRepository))]
public class PostgresRepository(DbContext dbContext) : IPostgresRepository
{
    public DbSet<T> GetQueryable<T>() where T : class
    {
        return dbContext.Set<T>();
    }
    
    public DbContext GetDbContext()
    {
        return dbContext;
    }
}