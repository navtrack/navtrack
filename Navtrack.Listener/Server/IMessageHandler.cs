using System.Threading.Tasks;

namespace Navtrack.Listener.Server
{
    public interface IMessageHandler
    {
        Task HandleMessage(Client client, INetworkStreamWrapper networkStream, byte[] bytes);
    }
}