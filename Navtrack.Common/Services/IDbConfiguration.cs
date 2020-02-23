using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Navtrack.Common.Services
{
    public interface IDbConfiguration
    {
        Task<string> Get<T>(Expression<Func<T, string>> expression);
        Task Save<T>(Expression<Func<T, string>> expression, string value);
    }
}