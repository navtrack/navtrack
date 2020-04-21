using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener
{
    public interface IListenerService
    {
        Task Execute(CancellationToken cancellationToken);
    }
}