using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Navtrack.Common.Services;

public interface ISettingProvider
{
    Task<T> Get<T>();
    Task Set<T>(T value);
    Task<X> Get<T, X>(Expression<Func<T, X>> expression);
    Task Save<T, X>(Expression<Func<T, X>> expression, X value);
}