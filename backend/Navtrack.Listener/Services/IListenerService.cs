using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Services;

public interface IListenerService
{
    Task Execute(CancellationToken cancellationToken);
}