using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public class MessageInput
{
    public ProtocolConnectionContext ConnectionContext { get; set; }
    public INetworkStreamWrapper NetworkStream { get; set; }
    public DataMessage DataMessage { get; set; }
}