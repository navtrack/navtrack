using System.Threading.Tasks;

namespace Navtrack.Shared.Library.Events;

public interface IEventHandler<in T>
{
    Task Handle(T payload);
}