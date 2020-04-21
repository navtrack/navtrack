using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server
{
    public interface IClientHandler
    {
        Task HandleClient(CancellationToken cancellationToken, Client client);
    }
}