using System.Threading.Tasks;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public interface IMessageHandler
{
    Task HandleMessage(Client client, INetworkStreamWrapper networkStream, byte[] bytes);
}