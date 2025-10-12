using Microsoft.EntityFrameworkCore;

namespace Navtrack.Database.Postgres;

public interface IPostgresRepository
{
    DbSet<T> GetQueryable<T>() where T : class;
    DbContext GetDbContext();
}