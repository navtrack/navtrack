using System.Net;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Listener.Services
{
    public interface IConnectionService
    {
        Task<Connection> NewConnection(IPEndPoint ipEndPoint);
        Task MarkConnectionAsClosed(Connection connection);
    }
}