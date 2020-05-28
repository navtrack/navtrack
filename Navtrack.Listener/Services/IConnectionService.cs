using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Listener.Services
{
    public interface IConnectionService
    {
        Task<ConnectionEntity> NewConnection(string endPoint);
        Task MarkConnectionAsClosed(ConnectionEntity connection);
    }
}