using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public interface IProtocolConnectionHandler
{
    Task HandleConnection(IProtocol protocol, TcpClient tcpClient, CancellationToken cancellationToken);
}