using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navtrack.DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void Add<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        Task SaveChanges();
        void Update<T>(T entity) where T : class;
    }
}