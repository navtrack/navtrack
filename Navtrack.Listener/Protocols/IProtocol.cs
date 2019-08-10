using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Protocols
{
    public interface IProtocol
    {
        Task HandleClient(TcpClient client, CancellationToken stoppingToken);
        int Port { get; }
    }
}