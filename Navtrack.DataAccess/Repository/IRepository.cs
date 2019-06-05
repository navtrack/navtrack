using System.Linq;

namespace Navtrack.DataAccess.Repository
{
    public interface IRepository
    {
        IQueryable<T> GetEntities<T>() where T : class;
    }
}