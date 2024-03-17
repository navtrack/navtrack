using System.Threading.Tasks;

namespace Navtrack.Shared.Library.Events;

public interface IPost
{
    Task Send<T>(T payload) where T : IEvent;
}