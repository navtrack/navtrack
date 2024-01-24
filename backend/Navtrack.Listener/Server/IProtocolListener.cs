using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server;

public interface IProtocolListener
{
    Task Start(IProtocol protocol, CancellationToken cancellationToken);
}