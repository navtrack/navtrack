using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Repository
{
    public interface ICustomDbContextFactory
    {
        DbContext CreateDbContext();
    }
}