using System.Net;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Common.Services
{
    public interface IConnectionService
    {
        Task<Connection> NewConnection(IPEndPoint ipEndPoint);
        Task MarkConnectionAsClosed(Connection connection);
    }
}