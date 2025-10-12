using Microsoft.EntityFrameworkCore;

namespace Navtrack.Database.Postgres;

public interface IPostgresDbContextProvider
{
    DbContext GetDbContext();
}