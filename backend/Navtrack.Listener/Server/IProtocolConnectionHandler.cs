using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server;

public interface IProtocolConnectionHandler
{
    Task HandleConnection(TcpClientAdapter tcpClient, IProtocol protocol, CancellationToken cancellationToken);
}