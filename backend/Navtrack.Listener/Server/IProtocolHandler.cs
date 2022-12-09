using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server;

public interface IProtocolHandler
{
    Task HandleProtocol(CancellationToken cancellationToken, IProtocol protocol);
}