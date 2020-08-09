using System.Threading.Tasks;

namespace Navtrack.DataAccess.Services
{
    public interface IGenericDataService<T>
    {
        Task Add(T entity);
    }
}