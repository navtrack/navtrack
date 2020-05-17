using System.Net;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Listener.Services
{
    public interface IConnectionService
    {
        Task<ConnectionEntity> NewConnection(IPEndPoint ipEndPoint);
        Task MarkConnectionAsClosed(ConnectionEntity connection);
    }
}