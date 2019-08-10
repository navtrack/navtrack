using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Protocols
{
    public interface IProtocol
    {
        Task HandleStream(NetworkStream networkStream, CancellationToken stoppingToken);
        int Port { get; }
    }
}