using System.Threading;
using System.Threading.Tasks;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public interface IProtocolConnectionHandler
{
    Task HandleConnection(ProtocolConnectionContext connectionContext, CancellationToken cancellationToken);
}