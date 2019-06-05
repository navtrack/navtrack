using System;
using System.Threading.Tasks;

namespace Navtrack.DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void Add<T>(T entity) where T : class;
        Task SaveChanges();
    }
}