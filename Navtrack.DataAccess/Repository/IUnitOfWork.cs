using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navtrack.DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void Add<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> locations) where T : class;
        Task SaveChanges();
    }
}