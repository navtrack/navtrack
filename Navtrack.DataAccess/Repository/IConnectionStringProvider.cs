using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Repository
{
    public interface IConnectionStringProvider
    {
        void ApplyConnectionString<T>(DbContextOptionsBuilder<T> optionsBuilder) where T : DbContext;
    }
}