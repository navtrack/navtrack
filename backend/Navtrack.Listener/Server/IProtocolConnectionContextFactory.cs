using System.Threading.Tasks;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public interface IProtocolConnectionContextFactory
{
    Task<ProtocolConnectionContext> GetConnectionContext(IProtocol protocol, TcpClientAdapter tcpClient);
}