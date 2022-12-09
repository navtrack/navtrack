using System.Threading;
using System.Threading.Tasks;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public interface IClientHandler
{
    Task HandleClient(CancellationToken cancellationToken, Client client);
}