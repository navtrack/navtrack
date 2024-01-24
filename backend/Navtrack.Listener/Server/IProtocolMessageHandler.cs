using System.Threading.Tasks;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public interface IProtocolMessageHandler
{
    Task HandleMessage(ProtocolConnectionContext connectionContext, INetworkStreamWrapper networkStream, byte[] bytes);
}