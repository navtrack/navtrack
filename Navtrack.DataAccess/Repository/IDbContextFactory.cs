using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Repository
{
    public interface IDbContextFactory
    {
        DbContext CreateDbContext();
    }
}