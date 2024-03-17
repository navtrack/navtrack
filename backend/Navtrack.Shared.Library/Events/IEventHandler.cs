using System.Threading.Tasks;

namespace Navtrack.Shared.Library.Events;

public interface IEventHandler<in T> where T : IEvent
{
    Task Handle(T payload);
}

public interface IEventHandler
{
    Task Handle(string type, object payload);
}