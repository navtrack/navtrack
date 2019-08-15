using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Repository
{
    public interface IInterceptorService
    {
        void InterceptChanges(DbContext dbContext);
    }
}