using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server
{
    public interface IStreamHandler
    {
        Task HandleStream(CancellationToken cancellationToken,
            Client client, INetworkStreamWrapper networkStream);
    }
}